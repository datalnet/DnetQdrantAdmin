using Azure.AI.OpenAI;
using Dnet.QdrantAdmin.Api.Infrasctructure.Factories;
using Dnet.QdrantAdmin.Api.Infrasctructure.Models;
using Dnet.QdrantAdmin.Api.Infrasctructure.Services;
using Dnet.QdrantAdmin.Api.Apis;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.Configure<LlmProviderConfig>(builder.Configuration.GetSection("LlmProviderConfig"));

builder.Services.Configure<QdrantConfig>(builder.Configuration.GetSection("QdrantConfig"));

builder.Services.AddSingleton<OpenAIClient>(sp =>
{
    var llmProviderConfig = sp.GetRequiredService<IOptions<LlmProviderConfig>>().Value;

    return new OpenAIClient(llmProviderConfig.ApiKey);
});

builder.Services.AddCors(
               options =>
               {
                   options.AddPolicy("CorsPolicy",
                       builder => builder
                           .WithOrigins(
                               "https://localhost:7188"
                               )
                           .AllowAnyMethod()
                           .AllowAnyHeader()
                           .Build());
               });

builder.Services.AddHttpClient();

builder.Services.AddSingleton<IOpenAiService, OpenAiService>();

builder.Services.AddTransient<IProblemDetailFactory, ProblemDetailFactory>();

builder.Services.AddScoped<IQdrantService, QdrantService>();

#pragma warning disable SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
var kernelBuilder = Kernel.CreateBuilder();
var llmProviderConfig = new LlmProviderConfig();
builder.Configuration.GetSection("LlmProviderConfig").Bind(llmProviderConfig);
kernelBuilder.AddOpenAITextEmbeddingGeneration(llmProviderConfig.Models[0].Model, llmProviderConfig.ApiKey);
#pragma warning restore SKEXP0010 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

var kernel = kernelBuilder.Build();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("CorsPolicy");

var qadrants = app.MapGroup("/api/Qdrants");
qadrants.QdrantsApis();

var llmProviders = app.MapGroup("/api/LlmProviders");
llmProviders.LlmProviderApis();

app.Run();













//AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
//var httpClientHandler = new HttpClientHandler();
//httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
//var qdrantConfig = new QdrantConfig();
//builder.Configuration.GetSection("QdrantConfig").Bind(qdrantConfig);
//var channel = GrpcChannel.ForAddress($"https://{qdrantConfig}", new GrpcChannelOptions { HttpHandler = httpClientHandler });