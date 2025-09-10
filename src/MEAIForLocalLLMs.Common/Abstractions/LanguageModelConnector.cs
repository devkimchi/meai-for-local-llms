using MEAIForLocalLLMs.Common.Configurations;
using MEAIForLocalLLMs.Common.Connectors;

using Microsoft.Extensions.AI;

namespace MEAIForLocalLLMs.Common.Abstractions;

/// <summary>
/// This represents the base language model connector entity for all language model connectors to inherit.
/// </summary>
public abstract class LanguageModelConnector(LanguageModelSettings? settings)
{
    /// <summary>
    /// Gets the <see cref="LanguageModelSettings"/> instance.
    /// </summary>
    protected LanguageModelSettings? Settings { get; } = settings;

    /// <summary>
    /// Gets an <see cref="IChatClient"/> instance.
    /// </summary>
    /// <returns>Returns <see cref="IChatClient"/> instance.</returns>
    public abstract Task<IChatClient> GetChatClientAsync();

    /// <summary>
    /// Gets an <see cref="IChatClient"/> instance based on the app settings provided.
    /// </summary>
    /// <param name="settings"><see cref="AppSettings"/> instance.</param>
    /// <returns>Returns <see cref="IChatClient"/> instance.</returns>
    public static async Task<IChatClient> CreateChatClientAsync(AppSettings settings)
    {
        LanguageModelConnector connector = settings.ConnectorType switch
        {
            ConnectorType.GitHubModels => new GitHubModelsConnector(settings),
            ConnectorType.DockerModelRunner => new DockerModelRunnerConnector(settings),
            ConnectorType.FoundryLocal => new FoundryLocalConnector(settings),
            ConnectorType.HuggingFace => new HuggingFaceConnector(settings),
            ConnectorType.Ollama => new OllamaConnector(settings),
            _ => throw new NotSupportedException($"Connector type '{settings.ConnectorType}' is not supported.")
        };

        return await connector.GetChatClientAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Logs the chat client initialization to the console.
    /// </summary>
    /// <param name="connector">The connector name.</param>
    /// <param name="model">The model name.</param>
    protected void LogChatClientInitialization(string connector, string model)
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine();
        Console.WriteLine($"{connector} client initialized with {model}.");
        Console.WriteLine();
        Console.ForegroundColor = foregroundColor;
    }
}
