using System.Text.Json.Serialization;

using MEAIForLocalLLMs.Common.Connectors;

namespace MEAIForLocalLLMs.Common.Configurations;

/// <summary>
/// This represents the app settings entity from appsettings.json.
/// </summary>
public partial class AppSettings
{
    /// <summary>
    /// Gets or sets the connector type to use.
    /// </summary>
    public ConnectorType ConnectorType { get; set; }

    /// <summary>
    /// Gets or sets the model name.
    /// </summary>
    [JsonIgnore]
    public string? Model { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether to use .NET Aspire or not.
    /// </summary>
    public bool UseAspire { get; set; }

    /// <summary>
    /// Gets or sets the value indicating whether to display help information or not.
    /// </summary>
    public bool Help { get; set; }
}
