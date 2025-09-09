using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Connectors;

var builder = DistributedApplication.CreateBuilder(args);

var config = builder.Configuration;
var settings = ArgumentOptions.Parse(config, args);
if (settings.Help == true)
{
    ArgumentOptions.DisplayHelp();
    return;
}

var webapp = builder.AddProject<Projects.MEAIForLocalLLMs_WebApp>("webapp");

if (settings.ConnectorType == ConnectorType.GitHubModels)
{
    var github = builder.AddGitHubModel("github", settings.Model!)
                        .WithHealthCheck();

    webapp.WithReference(github)
          .WithEnvironment("UseAspire", settings.UseAspire.ToString().ToLowerInvariant())
          .WaitFor(github);
}

builder.Build().Run();
