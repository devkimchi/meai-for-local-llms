using MEAIForLocalLLMs.Common.Abstractions;
using MEAIForLocalLLMs.Common.Connectors;
using MEAIForLocalLLMs.WebApp.Components;

using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;
var settings = ArgumentOptions.Parse(config, args);
if (settings.Help == true)
{
    ArgumentOptions.DisplayHelp();
    return;
}

builder.AddServiceDefaults();

builder.Services.AddSingleton(settings);
builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

if (settings.UseAspire == true)
{
    if (settings.ConnectorType == ConnectorType.GitHubModels)
    {
        builder.AddOpenAIClient("github")
               .AddChatClient()
               .UseFunctionInvocation()
               .UseLogging();
    }
}
else
{
    var chatClient = await LanguageModelConnector.CreateChatClientAsync(settings);

    builder.Services.AddChatClient(chatClient)
                    .UseFunctionInvocation()
                    .UseLogging();
}

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.UseStaticFiles();

app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

await app.RunAsync();
