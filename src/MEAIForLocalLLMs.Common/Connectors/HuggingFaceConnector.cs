using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Configurations;

using Microsoft.Extensions.AI;

using OllamaSharp;

namespace MEAIForLocalLLMs.Common.Connectors;

/// <summary>
/// This represents the connector entity for Hugging Face.
/// </summary>
/// <param name="settings"><see cref="AppSettings"/> instance.</param>
public class HuggingFaceConnector(AppSettings settings) : LanguageModelConnector(settings.HuggingFace)
{
    /// <inheritdoc/>
    public override async Task<IChatClient> GetChatClientAsync()
    {
        var settings = this.Settings as HuggingFaceSettings;

        var model = settings!.Model!;
        var client = new OllamaApiClient(new Uri(settings.BaseUrl!))
        {
            SelectedModel = model
        };
        var chatClient = client as IChatClient;

        this.LogChatClientInitialization("Hugging Face", model);

        return await Task.FromResult(chatClient).ConfigureAwait(false);
    }
}
