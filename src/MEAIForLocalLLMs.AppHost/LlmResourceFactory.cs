using Microsoft.Extensions.Configuration;

public static class LlmResourceFactory
{
    public static IResourceBuilder<ProjectResource> WithLlmReference(this IResourceBuilder<ProjectResource> source, IConfiguration config, IEnumerable<string> args)
    {
        var provider = config["LlmProvider"];
        foreach (var arg in args)
        {
            var index = args.ToList().IndexOf(arg);
            switch (arg)
            {
                case "--provider":
                    provider = args.ToList()[index + 1];
                    break;
            }
        }
        if (string.IsNullOrWhiteSpace(provider))
        {
            throw new InvalidOperationException("Missing configuration: LlmProvider");
        }

        source = provider switch
        {
            "AzureOpenAI" => source.AddAzureOpenAIResource(config, provider),
            "DockerModelRunner" => source.AddDockerModelRunnerResource(config, provider),
            "FoundryLocal" => source.AddFoundryLocalResource(config, provider),
            "GitHubModels" => source.AddGitHubModelsResource(config, provider),
            "HuggingFace" => source.AddHuggingFaceResource(config, provider),
            "Ollama" => source.AddOllamaResource(config, provider),
            _ => throw new NotSupportedException($"The specified LLM provider '{provider}' is not supported.")
        };

        return source;
    }

    private static IResourceBuilder<ProjectResource> AddAzureOpenAIResource(this IResourceBuilder<ProjectResource> source, IConfiguration config, string provider)
    {
        var azure = config.GetSection("Azure:OpenAI");
        var endpoint = azure["Endpoint"] ?? throw new InvalidOperationException("Missing configuration: Azure:OpenAI:Endpoint");
        var accessKey = azure["ApiKey"] ?? throw new InvalidOperationException("Missing configuration: Azure:OpenAI:ApiKey");
        var deploymentName = azure["DeploymentName"] ?? throw new InvalidOperationException("Missing configuration: Azure:OpenAI:DeploymentName");

        Console.WriteLine();
        Console.WriteLine($"\tUsing {provider}: {deploymentName}");
        Console.WriteLine();

        var apiKey = source.ApplicationBuilder
                           .AddParameter(name: "apiKey", value: accessKey, secret: true);
        var chat = source.ApplicationBuilder
                         .AddOpenAI("openai")
                         .WithEndpoint($"{endpoint.TrimEnd('/')}/openai/v1/")
                         .WithApiKey(apiKey)
                         .AddModel(name: "chat", model: deploymentName);

        return source.WithEnvironment("LlmProvider", provider)
                     .WithReference(chat)
                     .WaitFor(chat);
    }

    private static IResourceBuilder<ProjectResource> AddDockerModelRunnerResource(this IResourceBuilder<ProjectResource> source, IConfiguration config, string provider)
    {
        var dockerSection = config.GetSection("DockerModelRunner");
        var baseUrl = dockerSection["BaseUrl"] ?? throw new InvalidOperationException("Missing configuration: DockerModelRunner:BaseUrl");
        var model = dockerSection["Model"] ?? throw new InvalidOperationException("Missing configuration: DockerModelRunner:Model");

        Console.WriteLine();
        Console.WriteLine($"\tUsing {provider}: {model}");
        Console.WriteLine();

        var apiKey = source.ApplicationBuilder
                           .AddParameter(name: "apiKey", value: "docker-model-runner", secret: true);
        var docker = source.ApplicationBuilder
                           .AddOpenAI("docker")
                           .WithEndpoint(baseUrl)
                           .WithApiKey(apiKey);
        var chat = docker.AddModel(name: "chat", model: model);

        return source.WithEnvironment("LlmProvider", provider)
                     .WithReference(chat)
                     .WaitFor(chat);
    }

    private static IResourceBuilder<ProjectResource> AddFoundryLocalResource(this IResourceBuilder<ProjectResource> source, IConfiguration config, string provider)
    {
        var foundrySection = config.GetSection("FoundryLocal");
        var alias = foundrySection["Alias"] ?? throw new InvalidOperationException("Missing configuration: FoundryLocal:Alias");

        Console.WriteLine();
        Console.WriteLine($"\tUsing {provider}: {alias}");
        Console.WriteLine();

        var foundry = source.ApplicationBuilder
                            .AddAzureAIFoundry("foundry")
                            .RunAsFoundryLocal();
        var chat = foundry.AddDeployment(name: "chat",modelName: alias, modelVersion: "1", format: "Microsoft");

        return source.WithEnvironment("LlmProvider", provider)
                     .WithReference(chat)
                     .WaitFor(chat);
    }

    private static IResourceBuilder<ProjectResource> AddGitHubModelsResource(this IResourceBuilder<ProjectResource> source, IConfiguration config, string provider)
    {
        var github = config.GetSection("GitHub:Models");
        var endpoint = github["Endpoint"] ?? throw new InvalidOperationException("Missing configuration: GitHub:Models:Endpoint");
        var token = github["Token"] ?? throw new InvalidOperationException("Missing configuration: GitHub:Models:Token");
        var model = github["Model"] ?? throw new InvalidOperationException("Missing configuration: GitHub:Models:Model");

        Console.WriteLine();
        Console.WriteLine($"\tUsing {provider}: {model}");
        Console.WriteLine();

        var apiKey = source.ApplicationBuilder
                           .AddParameter(name: "apiKey", value: token, secret: true);
        var chat = source.ApplicationBuilder
                         .AddGitHubModel(name: "chat", model: model)
                         .WithApiKey(apiKey);

        return source.WithEnvironment("LlmProvider", provider)
                     .WithReference(chat)
                     .WaitFor(chat);
    }

    private static IResourceBuilder<ProjectResource> AddHuggingFaceResource(this IResourceBuilder<ProjectResource> source, IConfiguration config, string provider)
    {
        var hfSection = config.GetSection("HuggingFace");
        var model = hfSection["Model"] ?? throw new InvalidOperationException("Missing configuration: HuggingFace:Model");

        Console.WriteLine();
        Console.WriteLine($"\tUsing {provider}: {model}");
        Console.WriteLine();

        var hf = source.ApplicationBuilder
                       .AddOllama("hf")
                       .WithImageTag("latest")
                       .WithDataVolume();
        var chat = hf.AddHuggingFaceModel(name: "chat", modelName: model);

        return source.WithEnvironment("LlmProvider", provider)
                     .WithReference(chat)
                     .WaitFor(chat);
    }

    private static IResourceBuilder<ProjectResource> AddOllamaResource(this IResourceBuilder<ProjectResource> source, IConfiguration config, string provider)
    {
        var ollamaSection = config.GetSection("Ollama");
        var model = ollamaSection["Model"] ?? throw new InvalidOperationException("Missing configuration: Ollama:Model");

        Console.WriteLine();
        Console.WriteLine($"\tUsing {provider}: {model}");
        Console.WriteLine();

        var ollama = source.ApplicationBuilder
                           .AddOllama("ollama")
                           .WithImageTag("latest")
                           .WithDataVolume();
        var chat = ollama.AddModel(name: "chat", modelName: model);

        return source.WithEnvironment("LlmProvider", provider)
                     .WithReference(chat)
                     .WaitFor(chat);
    }
}
