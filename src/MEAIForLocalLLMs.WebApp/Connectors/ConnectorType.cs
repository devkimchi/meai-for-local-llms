using System.Text.Json.Serialization;

namespace MEAIForLocalLLMs.WebApp.Connectors;

/// <summary>
/// This specifies the type of connector to use.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ConnectorType
{
    /// <summary>
    /// Identifies the unknown connector type.
    /// </summary>
    Unknown,

    /// <summary>
    /// Identifies the Docker Model Runner connector type.
    /// </summary>
    DockerModelRunner,

    /// <summary>
    /// Identifies the Foundry Local connector type.
    /// </summary>
    FoundryLocal,

    /// <summary>
    /// Identifies the GitHub Models connector type.
    /// </summary>
    GitHubModels,

    /// <summary>
    /// Identifies the Hugging Face connector type.
    /// </summary>
    HuggingFace,

    /// <summary>
    /// Identifies the Ollama connector type.
    /// </summary>
    Ollama,
}