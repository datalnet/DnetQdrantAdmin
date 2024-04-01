using Dnet.QdrantAdmin.Client.Infrastructure;
using Dnet.QdrantAdmin.Client.Infrastructure.Interceptor;
using Dnet.Blazor.Infrastructure.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<StatusCodeHttpMessageHandler>();

builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<StatusCodeHttpMessageHandler>();

    return new HttpClient(handler)
    {
        BaseAddress = new Uri(builder.Configuration["QDRANTADMIN_API_URL"])
    };
});

builder.Services.AddBlazeOrbitalClient();

builder.Services.AddDnetBlazor();

await builder.Build().RunAsync();
