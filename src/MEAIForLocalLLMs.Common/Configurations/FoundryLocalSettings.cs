using MEAIForLocalLLMs.Common.Abstractions;

namespace MEAIForLocalLLMs.Common.Configurations;

/// <inheritdoc/>
public partial class AppSettings
{
    /// <summary>
    /// Gets or sets the <see cref="FoundryLocalSettings"/> instance.
    /// </summary>
    public FoundryLocalSettings? FoundryLocal { get; set; }
}

/// <summary>
/// This represents the app settings entity for Foundry Local.
/// </summary>
public class FoundryLocalSettings : LanguageModelSettings
{
    /// <summary>
    /// Gets or sets the model alias of Foundry Local.
    /// </summary>
    public string? Alias { get; set; }
}