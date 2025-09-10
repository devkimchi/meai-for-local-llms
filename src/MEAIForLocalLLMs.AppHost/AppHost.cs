using MEAIForLocalLLMs.AppHost.Extensions;
using MEAIForLocalLLMs.Common.Abstractions;

var builder = DistributedApplication.CreateBuilder(args);

var config = builder.Configuration;
var settings = ArgumentOptions.Parse(config, args);
if (settings.Help == true)
{
    ArgumentOptions.DisplayHelp();
    return;
}

var webapp = builder.AddProject<Projects.MEAIForLocalLLMs_WebApp>("webapp")
                    .WithLanguageModel(settings);

builder.Build().Run();
