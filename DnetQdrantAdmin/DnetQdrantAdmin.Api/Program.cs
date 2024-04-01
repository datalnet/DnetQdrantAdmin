using Azure.AI.OpenAI;
using Dnet.QdrantAdmin.Api.Infrasctructure.Factories;
using Dnet.QdrantAdmin.Api.Infrasctructure.Models;
using Dnet.QdrantAdmin.Api.Infrasctructure.Services;
using Dnet.QdrantAdmin.Api.Apis;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;

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

//builder.Services.AddScoped<IQdrantService, QdrantService>(provider =>
//    new QdrantService("192.168.1.44"));

builder.Services.AddScoped<IQdrantService, QdrantService>();

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
var httpClientHandler = new HttpClientHandler();
httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
var channel = GrpcChannel.ForAddress($"https://192.168.1.44:6334", new GrpcChannelOptions { HttpHandler = httpClientHandler });


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