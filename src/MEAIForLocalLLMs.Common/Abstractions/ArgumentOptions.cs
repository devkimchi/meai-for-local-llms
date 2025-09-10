using MEAIForLocalLLMs.Common.Configurations;
using MEAIForLocalLLMs.Common.Connectors;
using MEAIForLocalLLMs.Common.Options;

using Microsoft.Extensions.Configuration;

namespace MEAIForLocalLLMs.Common.Abstractions;

/// <summary>
/// This represents the base argument options settings entity for all arguments options to inherit.
/// </summary>
public abstract class ArgumentOptions
{
    private static readonly (ConnectorType ConnectorType, string Argument, bool IsSwitch)[] arguments =
    [
        // GitHub Models
        (ConnectorType.GitHubModels, "--token", false),
        (ConnectorType.GitHubModels, "--model", false),
        // Docker Model Runner
        (ConnectorType.DockerModelRunner, "--model", false),
        // Foundry Local
        (ConnectorType.FoundryLocal, "--alias", false),
        // Hugging Face
        (ConnectorType.HuggingFace, "--model", false),
        // Ollama
        (ConnectorType.Ollama, "--model", false)
    ];

    /// <summary>
    /// Gets or sets the connector type to use.
    /// </summary>
    public ConnectorType ConnectorType { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether to display help information or not.
    /// </summary>
    public bool Help { get; set; }

    /// <summary>
    /// Verifies the connector type from the configuration and command line arguments.
    /// </summary>
    /// <param name="config"><see cref="IConfiguration"/> instance.</param>
    /// <param name="args">List of arguments from the command line.</param>
    /// <returns>The verified <see cref="ConnectorType"/> value.</returns>
    public static ConnectorType VerifyConnectorType(IConfiguration config, string[] args)
    {
        var connectorType = Enum.TryParse<ConnectorType>(config["ConnectorType"], ignoreCase: true, out var result) ? result : ConnectorType.Unknown;
        for (var i = 0; i < args.Length; i++)
        {
            if (string.Equals(args[i], "--connector-type", StringComparison.InvariantCultureIgnoreCase) ||
                string.Equals(args[i], "-c", StringComparison.InvariantCultureIgnoreCase))
            {
                if (i + 1 < args.Length && Enum.TryParse<ConnectorType>(args[i + 1], ignoreCase: true, out result))
                {
                    connectorType = result;
                }
                break;
            }
        }

        return connectorType;
    }

    /// <summary>
    /// Parses the command line arguments into the specified options type.
    /// </summary>
    /// <param name="config"><see cref="IConfiguration"/> instance.</param>
    /// <param name="args">List of arguments from the command line.</param>
    /// <returns>The parsed options.</returns>
    public static AppSettings Parse(IConfiguration config, string[] args)
    {
        var settings = new AppSettings();
        config.Bind(settings);

        var connectorType = VerifyConnectorType(config, args);
        if (connectorType == ConnectorType.Unknown)
        {
            settings.ConnectorType = connectorType;
            settings.Help = true;

            return settings;
        }

        var expectedName = connectorType + "ArgumentOptions";

        var assembly = typeof(ArgumentOptions).Assembly;
        var optionsType = assembly?.GetTypes()
                                   .SingleOrDefault(t => typeof(ArgumentOptions).IsAssignableFrom(t) &&
                                                         string.Equals(t.Name, expectedName, StringComparison.InvariantCultureIgnoreCase))
                          ?? throw new InvalidOperationException($"Options type '{expectedName}' not found for connector type '{connectorType}'.");

        var options = (ArgumentOptions)Activator.CreateInstance(optionsType)!;
        options.ConnectorType = connectorType;

        options.ParseOptions(config, args);

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--connector-type":
                case "-c":
                    if (i + 1 < args.Length)
                    {
                        if (Enum.TryParse<ConnectorType>(args[++i], ignoreCase: true, out var result))
                        {
                            options.ConnectorType = result;
                        }
                    }
                    break;

                case "--help":
                case "-h":
                    options.Help = true;
                    break;

                default:
                    var argument = arguments.SingleOrDefault(p => p.ConnectorType == connectorType &&
                                                                  p.Argument.Equals(args[i], StringComparison.InvariantCultureIgnoreCase));
                    if (argument == default)
                    {
                        options.Help = true;
                    }
                    else if (argument.IsSwitch == false)
                    {
                        i++;
                    }
                    break;
            }
        }

        switch (options)
        {
            case GitHubModelsArgumentOptions github:
                settings.GitHubModels ??= new GitHubModelsSettings();
                settings.GitHubModels.Token = github.Token ?? settings.GitHubModels.Token;
                settings.GitHubModels.Model = github.Model ?? settings.GitHubModels.Model;
                settings.Model = github.Model ?? settings.GitHubModels.Model;
                break;

            case DockerModelRunnerArgumentOptions docker:
                settings.DockerModelRunner ??= new DockerModelRunnerSettings();
                settings.DockerModelRunner.Model = docker.Model ?? settings.DockerModelRunner.Model;
                settings.Model = docker.Model ?? settings.DockerModelRunner.Model;
                break;

            case FoundryLocalArgumentOptions foundry:
                settings.FoundryLocal ??= new FoundryLocalSettings();
                settings.FoundryLocal.Alias = foundry.Alias ?? settings.FoundryLocal.Alias;
                settings.Model = foundry.Alias ?? settings.FoundryLocal.Alias;
                break;

            case HuggingFaceArgumentOptions hf:
                settings.HuggingFace ??= new HuggingFaceSettings();
                settings.HuggingFace.Model = hf.Model ?? settings.HuggingFace.Model;
                settings.Model = hf.Model ?? settings.HuggingFace.Model;
                break;

            case OllamaArgumentOptions ollama:
                settings.Ollama ??= new OllamaSettings();
                settings.Ollama.Model = ollama.Model ?? settings.Ollama.Model;
                settings.Model = ollama.Model ?? settings.Ollama.Model;
                break;

            default:
                break;
        }

        settings.ConnectorType = options.ConnectorType;
        settings.Help = options.ShouldDisplayHelp(optionsType);

        return settings;
    }

    /// <summary>
    /// Displays the help information for the command line arguments.
    /// </summary>
    public static void DisplayHelp()
    {
        var foregroundColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("OpenChat Playground");
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine("Usage: [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --connector-type|-c  The connector type. Supporting connectors are:");
        Console.WriteLine("                       - GitHubModels");
        Console.WriteLine("                       - DockerModelRunner, FoundryLocal, HuggingFace, Ollama");
        Console.WriteLine();
        DisplayHelpForGitHubModels();
        DisplayHelpForDockerModelRunner();
        DisplayHelpForFoundryLocal();
        DisplayHelpForHuggingFace();
        DisplayHelpForOllama();
        Console.WriteLine("  --help|-h            Show this help message.");
    }

    /// <summary>
    /// Parses the command line arguments into the specified options type.
    /// </summary>
    /// <param name="config"><see cref="IConfiguration"/> instance.</param>
    /// <param name="args">List of arguments from the command line.</param>
    protected virtual void ParseOptions(IConfiguration config, string[] args)
    {
        return;
    }

    /// <summary>
    /// Determines whether to display help information based on the options provided.
    /// </summary>
    /// <param name="type">The type of the options to parse into.</param>
    /// <returns></returns>
    protected virtual bool ShouldDisplayHelp(Type type)
    {
        return this.ConnectorType == ConnectorType.Unknown || this.Help;
    }

    private static void DisplayHelpForGitHubModels()
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("  ** GitHub Models: **");
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine("  --token              The GitHub PAT.");
        Console.WriteLine("  --model              The model name. Default to 'openai/gpt-4o-mini'");
        Console.WriteLine();
    }

    private static void DisplayHelpForDockerModelRunner()
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  ** Docker Model Runner: **");
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine("  --model              The model name. Default to 'ai/gpt-oss'");
        Console.WriteLine();
    }

    private static void DisplayHelpForFoundryLocal()
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  ** Foundry Local: **");
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine("  --alias              The model alias. Default to 'gpt-oss-20b'");
        Console.WriteLine();
    }

    private static void DisplayHelpForHuggingFace()
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  ** Hugging Face: **");
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine("  --model              The model name. Default to 'hf.co/LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF'");
        Console.WriteLine();
    }

    private static void DisplayHelpForOllama()
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  ** Ollama: **");
        Console.ForegroundColor = foregroundColor;

        Console.WriteLine("  --model              The model name. Default to 'gpt-oss'");
        Console.WriteLine();
    }
}
