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
            // ConnectorType.DockerModelRunner => builder.AddDockerModelRunner(settings),
            ConnectorType.FoundryLocal => builder.AddFoundryLocal(settings),
            ConnectorType.HuggingFace => builder.AddHuggingFace(settings),
            ConnectorType.Ollama => builder.AddOllama(settings),
            _ => throw new NotSupportedException($"Connector type '{settings.ConnectorType}' is not supported."),
        };

        return builder;
    }

    public static WebApplicationBuilder AddGitHubModels(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOpenAIClient("github")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        return builder;
    }

    public static WebApplicationBuilder AddDockerModelRunner(this WebApplicationBuilder builder, AppSettings settings)
    {
        return builder;
    }

    public static WebApplicationBuilder AddFoundryLocal(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOpenAIClient("foundry-local")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        return builder;
    }

    public static WebApplicationBuilder AddHuggingFace(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOllamaApiClient("model")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        return builder;
    }

    public static WebApplicationBuilder AddOllama(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.AddOllamaApiClient($"ollama-{settings.Model!}")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();

        return builder;
    }
}
