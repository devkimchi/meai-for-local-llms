using System.Text.Json.Serialization;

using MEAIForLocalLLMs.WebApp.Abstractions;

namespace MEAIForLocalLLMs.WebApp.Configurations;

/// <inheritdoc/>
public partial class AppSettings
{
    /// <summary>
    /// Gets or sets the <see cref="DockerModelRunnerSettings"/> instance.
    /// </summary>
    public DockerModelRunnerSettings? DockerModelRunner { get; set; }
}

/// <summary>
/// This represents the app settings entity for Docker Model Runner.
/// </summary>
public class DockerModelRunnerSettings : LanguageModelSettings
{
    /// <summary>
    /// Gets or sets the base URL of Docker Model Runner API.
    /// </summary>
    public string? BaseUrl { get; set; }

    /// <summary>
    /// Gets the Docker Model Runner API key.
    /// </summary>
    [JsonIgnore]
    public string? ApiKey { get; } = "docker-model-runner-key-ignore";

    /// <summary>
    /// Gets or sets the model name of Docker Model Runner.
    /// </summary>
    public string? Model { get; set; }
}