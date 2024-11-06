using Dnet.QdrantAdmin.Api.Apis;
using Dnet.QdrantAdmin.Api.Infrasctructure.Factories;
using Dnet.QdrantAdmin.Api.Infrasctructure.Models;
using Dnet.QdrantAdmin.Api.Infrasctructure.Services;
using Microsoft.SemanticKernel;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services.Configure<LlmProviderConfig>(builder.Configuration.GetSection("LlmProviderConfig"));

builder.Services.Configure<QdrantConfig>(builder.Configuration.GetSection("QdrantConfig"));

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

var llmProviderConfig = new LlmProviderConfig();
builder.Configuration.GetSection("LlmProviderConfig").Bind(llmProviderConfig);

var kernel = builder.Services.AddTransient<Kernel>((sp) =>
{
    var factory = sp.GetRequiredService<IHttpClientFactory>();

#pragma warning disable SKEXP0010
    return Kernel.CreateBuilder()
        .AddOpenAITextEmbeddingGeneration(llmProviderConfig.Models[0].Model, llmProviderConfig.ApiKey)
        .Build();
#pragma warning restore SKEXP0010
});

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