using System.ClientModel;

using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Configurations;

using Microsoft.Extensions.AI;

using OpenAI;

namespace MEAIForLocalLLMs.Common.Connectors;

/// <summary>
/// This represents the connector entity for Docker Model Runner.
/// </summary>
/// <param name="settings"><see cref="AppSettings"/> instance.</param>
public class DockerModelRunnerConnector(AppSettings settings) : LanguageModelConnector(settings.DockerModelRunner)
{
    /// <inheritdoc/>
    public override async Task<IChatClient> GetChatClientAsync()
    {
        var settings = this.Settings as DockerModelRunnerSettings;

        var model = settings!.Model!;
        var credential = new ApiKeyCredential(settings.ApiKey!);
        var options = new OpenAIClientOptions()
        {
            Endpoint = new Uri(settings.BaseUrl!)
        };
        var client = new OpenAIClient(credential, options);
        var chatClient = client.GetChatClient(model)
                               .AsIChatClient();

        this.LogChatClientInitialization("Docker Model Runner", model);

        return await Task.FromResult(chatClient).ConfigureAwait(false);
    }
}
