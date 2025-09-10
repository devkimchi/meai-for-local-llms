using System.Text;

using MEAIForLocalLLMs.Common.Configurations;
using MEAIForLocalLLMs.Common.Connectors;

namespace MEAIForLocalLLMs.AppHost.Extensions;

public static class LanguageModelReferenceExtensions
{
    public static IResourceBuilder<ProjectResource> WithLanguageModel(this IResourceBuilder<ProjectResource> project, AppSettings settings)
    {
        project = settings.ConnectorType switch
        {
            ConnectorType.GitHubModels => project.ApplicationBuilder.AddGitHubModels(project, settings),
            ConnectorType.DockerModelRunner => project.ApplicationBuilder.AddDockerModelRunner(project, settings),
            ConnectorType.FoundryLocal => project.ApplicationBuilder.AddFoundryLocal(project, settings),
            ConnectorType.HuggingFace => project.ApplicationBuilder.AddHuggingFace(project, settings),
            ConnectorType.Ollama => project.ApplicationBuilder.AddOllama(project, settings),
            _ => throw new NotSupportedException($"Connector type '{settings.ConnectorType}' is not supported."),
        };

        project.WithEnvironment("UseAspire", settings.UseAspire.ToString().ToLowerInvariant())
               .WithEnvironment("ConnectorType", settings.ConnectorType.ToString());

        return project;
    }

    public static IResourceBuilder<ProjectResource> AddGitHubModels(this IDistributedApplicationBuilder builder, IResourceBuilder<ProjectResource> project, AppSettings settings)
    {
        var github = builder.AddGitHubModel("github-models", settings.Model!)
                            .WithHealthCheck();

        project.WithReference(github)
               .WaitFor(github)
               .WithEnvironment("GitHubModels:Model", settings.GitHubModels?.Model);

        LogModelIntegration("GitHub Models", settings.Model!);

        return project;
    }

    public static IResourceBuilder<ProjectResource> AddDockerModelRunner(this IDistributedApplicationBuilder builder, IResourceBuilder<ProjectResource> project, AppSettings settings)
    {
        var sb = new StringBuilder();
        sb.AppendFormat("Endpoint={0};Key={1};DeploymentId={2};Model={3}",
                        settings.DockerModelRunner?.BaseUrl,
                        settings.DockerModelRunner?.ApiKey,
                        settings.DockerModelRunner?.Model,
                        settings.DockerModelRunner?.Model);
        var connectionString = sb.ToString();

        var docker = builder.AddConnectionString("docker-model-runner", builder => builder.AppendLiteral(connectionString));

        project.WithReference(docker)
               .WaitFor(docker)
               .WithEnvironment("DockerModelRunner:Model", settings.DockerModelRunner?.Model);

        LogModelIntegration("Docker Model Runner", settings.Model!);

        return project;
    }

    public static IResourceBuilder<ProjectResource> AddFoundryLocal(this IDistributedApplicationBuilder builder, IResourceBuilder<ProjectResource> project, AppSettings settings)
    {
        var foundry = builder.AddAzureAIFoundry("foundry")
                             .RunAsFoundryLocal();
        var foundryLocal = foundry.AddDeployment("foundry-local", settings.Model!, "1", "Microsoft");

        project.WithReference(foundryLocal)
               .WaitFor(foundryLocal)
               .WithEnvironment("FoundryLocal:Alias", settings.FoundryLocal?.Alias);

        LogModelIntegration("Foundry Local", settings.Model!);

        return project;
    }

    public static IResourceBuilder<ProjectResource> AddHuggingFace(this IDistributedApplicationBuilder builder, IResourceBuilder<ProjectResource> project, AppSettings settings)
    {
        var hf = builder.AddOllama("hf")
                        .WithImageTag("latest")
                        .WithDataVolume();
        var model = hf.AddHuggingFaceModel("hugging-face", settings.Model!);

        project.WithReference(model)
               .WaitFor(model)
               .WithEnvironment("HuggingFace:Model", settings.HuggingFace?.Model);

        LogModelIntegration("Hugging Face", settings.Model!);

        return project;
    }

    public static IResourceBuilder<ProjectResource> AddOllama(this IDistributedApplicationBuilder builder, IResourceBuilder<ProjectResource> project, AppSettings settings)
    {
        var ollama = builder.AddOllama("ollama")
                            .WithImageTag("latest")
                            .WithDataVolume();
        var model = ollama.AddModel(settings.Model!);

        project.WithReference(model)
               .WaitFor(model)
               .WithEnvironment("Ollama:Model", settings.Ollama?.Model);

        LogModelIntegration("Ollama", settings.Model!);

        return project;
    }

    private static void LogModelIntegration(string connector, string model)
    {
        var foregroundColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine();
        Console.WriteLine($"{connector} initialized with {model}.");
        Console.WriteLine();
        Console.ForegroundColor = foregroundColor;
    }
}
