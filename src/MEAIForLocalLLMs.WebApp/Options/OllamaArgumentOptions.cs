using MEAIForLocalLLMs.WebApp.Abstractions;
using MEAIForLocalLLMs.WebApp.Configurations;

namespace MEAIForLocalLLMs.WebApp.Options;

/// <summary>
/// This represents the argument options entity for Ollama.
/// </summary>
public class OllamaArgumentOptions : ArgumentOptions
{
    /// <summary>
    /// Gets or sets the model name of Ollama.
    /// </summary>
    public string? Model { get; set; }

    /// <inheritdoc/>
    protected override void ParseOptions(IConfiguration config, string[] args)
    {
        var settings = new AppSettings();
        config.Bind(settings);

        var ollama = settings.Ollama;

        this.Model ??= ollama?.Model;

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
