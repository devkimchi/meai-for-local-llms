using MEAIForLocalLLMs.WebApp.Abstractions;
using MEAIForLocalLLMs.WebApp.Configurations;

using Microsoft.Extensions.AI;

using OllamaSharp;

namespace MEAIForLocalLLMs.WebApp.Connectors;

/// <summary>
/// This represents the connector entity for Ollama.
/// </summary>
/// <param name="settings"><see cref="AppSettings"/> instance.</param>
public class OllamaConnector(AppSettings settings) : LanguageModelConnector(settings.Ollama)
{
    /// <inheritdoc/>
    public override async Task<IChatClient> GetChatClientAsync()
    {
        var settings = this.Settings as OllamaSettings;

        var model = settings!.Model!;
        var client = new OllamaApiClient(new Uri(settings.BaseUrl!))
        {
            SelectedModel = model
        };
        var chatClient = client as IChatClient;

        this.LogChatClientInitialization("Ollama", model);

        return await Task.FromResult(chatClient).ConfigureAwait(false);
    }
}
