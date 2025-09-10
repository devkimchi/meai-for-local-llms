using MEAIForLocalLLMs.Common.Configurations;
using MEAIForLocalLLMs.Common.Connectors;

using Microsoft.Extensions.AI;

namespace MEAIForLocalLLMs.WebApp.Extensions;

public static class LanguageModelConnectorExtensions
{
    public static WebApplicationBuilder AddChatClient(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder = settings.ConnectorType switch
        {
            ConnectorType.GitHubModels => builder.AddGitHubModels(settings),
            ConnectorType.DockerModelRunner => builder.AddDockerModelRunner(settings),
            ConnectorType.FoundryLocal => builder.AddFoundryLocal(settings),
            ConnectorType.HuggingFace => builder.AddHuggingFace(settings),
            ConnectorType.Ollama => builder.AddOllama(settings),
            _ => throw new NotSupportedException($"Connector type '{settings.ConnectorType}' is not supported."),
        };

        return builder;
    }

    public static WebApplicationBuilder AddGitHubModels(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOpenAIClient("github-models")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        LogChatClientInitialization("GitHub Models", settings.Model!);

        return builder;
    }

    public static WebApplicationBuilder AddDockerModelRunner(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOpenAIClient("docker-model-runner")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        LogChatClientInitialization("Docker Model Runner", settings.Model!);

        return builder;
    }

    public static WebApplicationBuilder AddFoundryLocal(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOpenAIClient("foundry-local")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        LogChatClientInitialization("Foundry Local", settings.Model!);

        return builder;
    }

    public static WebApplicationBuilder AddHuggingFace(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOllamaApiClient("hugging-face")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        LogChatClientInitialization("Hugging Face", settings.Model!);

        return builder;
    }

    public static WebApplicationBuilder AddOllama(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOllamaApiClient($"ollama-{settings.Model!}")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        LogChatClientInitialization("Ollama", settings.Model!);

        return builder;
    }

    private static void LogChatClientInitialization(string connector, string model)
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine();
        Console.WriteLine($"{connector} client initialized with {model}.");
        Console.WriteLine();
        Console.ForegroundColor = foregroundColor;
    }
}
