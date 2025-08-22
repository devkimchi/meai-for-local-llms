using System.ClientModel;

using MEAIForLocalLLMs.WebApp.Components;

using Microsoft.AI.Foundry.Local;
using Microsoft.Extensions.AI;

using OllamaSharp;

using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

// GitHub Models
// var credential = new ApiKeyCredential(builder.Configuration["GitHubModels:Token"] ?? throw new InvalidOperationException("Missing configuration: GitHubModels:Token. See the README for details."));
// var openAIOptions = new OpenAIClientOptions()
// {
//     Endpoint = new Uri("https://models.inference.ai.azure.com")
// };

// var ghModelsClient = new OpenAIClient(credential, openAIOptions);
// var chatClient = ghModelsClient.GetChatClient("gpt-4o-mini").AsIChatClient();

// Ollama
// var ollamaClient = new OllamaApiClient(new Uri("http://localhost:11434"))
// {
//     SelectedModel = "gemma3"
// };
// var chatClient = ollamaClient as IChatClient;

// Hugging Face
// var hfClient = new OllamaApiClient(new Uri("http://localhost:11434"))
// {
//     SelectedModel = "hf.co/LGAI-EXAONE/EXAONE-4.0-1.2B-GGUF"
// };
// var chatClient = hfClient as IChatClient;

// Docker Model Runner
var model = "ai/gpt-oss";
var credential = new ApiKeyCredential("docker-model-runner-key");
var openAIOptions = new OpenAIClientOptions()
{
    Endpoint = new Uri("http://localhost:12434/engines/v1")
};
var dockerClient = new OpenAIClient(credential, openAIOptions);
var chatClient = dockerClient.GetChatClient(model).AsIChatClient();

Console.WriteLine($"Docker Model Runner client initialized with {model}.");

// Foundry Local
// var alias = "phi-4";
// var manager = await FoundryLocalManager.StartModelAsync(alias);
// var model = await manager.GetModelInfoAsync(alias);
// var credential = new ApiKeyCredential(manager.ApiKey);
// var openAIOptions = new OpenAIClientOptions
// {
//     Endpoint = manager.Endpoint
// };
// var foundryLocalClient = new OpenAIClient(credential, openAIOptions);
// var chatClient = foundryLocalClient.GetChatClient(model?.ModelId).AsIChatClient();

builder.Services.AddChatClient(chatClient);

var app = builder.Build();

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
