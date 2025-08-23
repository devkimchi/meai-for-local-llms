using MEAIForLocalLLMs.WebApp.Abstractions;
using MEAIForLocalLLMs.WebApp.Configurations;

namespace MEAIForLocalLLMs.WebApp.Options;

/// <summary>
/// This represents the argument options entity for Hugging Face.
/// </summary>
public class HuggingFaceArgumentOptions : ArgumentOptions
{
    /// <summary>
    /// Gets or sets the model name of Hugging Face.
    /// </summary>
    public string? Model { get; set; }

    /// <inheritdoc/>
    protected override void ParseOptions(IConfiguration config, string[] args)
    {
        var settings = new AppSettings();
        config.Bind(settings);

        var hf = settings.HuggingFace;

        this.Model ??= hf?.Model;

        for (var i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
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
