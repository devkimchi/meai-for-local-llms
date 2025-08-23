using System.ClientModel;

using MEAIForLocalLLMs.WebApp.Abstractions;
using MEAIForLocalLLMs.WebApp.Configurations;

using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.AI;

using OpenAI;

namespace MEAIForLocalLLMs.WebApp.Connectors;

/// <summary>
/// This represents the connector entity for Foundry Local.
/// </summary>
/// <param name="settings"><see cref="AppSettings"/> instance.</param>
public class FoundryLocalConnector(AppSettings settings) : LanguageModelConnector(settings.FoundryLocal)
{
    /// <inheritdoc/>
    public override async Task<IChatClient> GetChatClientAsync()
    {
        var settings = this.Settings as FoundryLocalSettings;

        var alias = settings!.Alias!;
        var manager = await FoundryLocalManager.StartModelAsync(alias).ConfigureAwait(false);
        var model = await manager.GetModelInfoAsync(alias).ConfigureAwait(false);
        var credential = new ApiKeyCredential(manager.ApiKey);
        var options = new OpenAIClientOptions
        {
            Endpoint = manager.Endpoint
        };
        var client = new OpenAIClient(credential, options);
        var chatClient = client.GetChatClient(model!.ModelId)
                               .AsIChatClient();

        this.LogChatClientInitialization("Foundry Local", model!.ModelId);

        return chatClient;
    }
}