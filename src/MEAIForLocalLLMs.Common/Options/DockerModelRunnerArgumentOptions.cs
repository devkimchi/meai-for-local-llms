using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Configurations;

using Microsoft.Extensions.Configuration;

namespace MEAIForLocalLLMs.Common.Options;

/// <summary>
/// This represents the argument options entity for Docker Model Runner.
/// </summary>
public class DockerModelRunnerArgumentOptions : ArgumentOptions
{
    /// <summary>
    /// Gets or sets the model name of Docker Model Runner.
    /// </summary>
    public string? Model { get; set; }

    /// <inheritdoc/>
    protected override void ParseOptions(IConfiguration config, string[] args)
    {
        var settings = new AppSettings();
        config.Bind(settings);

        var docker = settings.DockerModelRunner;

        this.Model ??= docker?.Model;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--model":
                    if (i + 1 < args.Length)
                    {
                        this.Model = args[++i];
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
