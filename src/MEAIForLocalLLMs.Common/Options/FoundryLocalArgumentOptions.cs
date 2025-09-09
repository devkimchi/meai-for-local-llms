using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Configurations;

using Microsoft.Extensions.Configuration;

namespace MEAIForLocalLLMs.Common.Options;

/// <summary>
/// This represents the argument options entity for Foundry Local.
/// </summary>
public class FoundryLocalArgumentOptions : ArgumentOptions
{
    /// <summary>
    /// Gets or sets the model alias of Foundry Local.
    /// </summary>
    public string? Alias { get; set; }

    /// <inheritdoc/>
    protected override void ParseOptions(IConfiguration config, string[] args)
    {
        var settings = new AppSettings();
        config.Bind(settings);

        var foundry = settings.FoundryLocal;

        this.Alias ??= foundry?.Alias;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--alias":
                    if (i + 1 < args.Length)
                    {
                        this.Alias = args[++i];
                    }
                    break;

                default:
                    break;
            }
        }
    }
}