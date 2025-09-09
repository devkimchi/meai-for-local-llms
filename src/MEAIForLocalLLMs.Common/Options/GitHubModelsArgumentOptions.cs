using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Configurations;

using Microsoft.Extensions.Configuration;

namespace MEAIForLocalLLMs.Common.Options;

/// <summary>
/// This represents the argument options entity for GitHub Models.
/// </summary>
public class GitHubModelsArgumentOptions : ArgumentOptions
{
    /// <summary>
    /// Gets or sets the personal access token for GitHub Models.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the model name of GitHub Models.
    /// </summary>
    public string? Model { get; set; }

    /// <inheritdoc/>
    protected override void ParseOptions(IConfiguration config, string[] args)
    {
        var settings = new AppSettings();
        config.Bind(settings);

        var github = settings.GitHubModels;

        this.Token ??= github?.Token;
        this.Model ??= github?.Model;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--token":
                    if (i + 1 < args.Length)
                    {
                        this.Token = args[++i];
                    }
                    break;

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
