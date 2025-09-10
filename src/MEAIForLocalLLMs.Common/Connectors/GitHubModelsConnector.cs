using System.ClientModel;

using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Configurations;

using Microsoft.Extensions.AI;

using OpenAI;

namespace MEAIForLocalLLMs.Common.Connectors;

/// <summary>
/// This represents the connector entity for GitHub Models.
/// </summary>
/// <param name="settings"><see cref="AppSettings"/> instance.</param>
public class GitHubModelsConnector(AppSettings settings) : LanguageModelConnector(settings.GitHubModels)
{
    /// <inheritdoc/>
    public override async Task<IChatClient> GetChatClientAsync()
    {
        var settings = this.Settings as GitHubModelsSettings;

        var model = settings!.Model!;
        var credential = new ApiKeyCredential(settings?.Token ?? throw new InvalidOperationException("Missing configuration: GitHubModels:Token."));
        var options = new OpenAIClientOptions()
        {
            Endpoint = new Uri(settings.Endpoint ?? throw new InvalidOperationException("Missing configuration: GitHubModels:Endpoint."))
        };

        var client = new OpenAIClient(credential, options);
        var chatClient = client.GetChatClient(model)
                               .AsIChatClient();

        this.LogChatClientInitialization("GitHub Models", model);

        return await Task.FromResult(chatClient).ConfigureAwait(false);
    }
}
