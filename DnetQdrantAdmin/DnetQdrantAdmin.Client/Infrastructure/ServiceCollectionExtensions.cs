using Dnet.QdrantAdmin.Client.Infrastructure.Services;
using Dnet.QdrantAdmin.Client.Pages.Admin;
using Dnet.QdrantAdmin.Client.Pages.TherapyNotes;

namespace Dnet.QdrantAdmin.Client.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazeOrbitalClient(this IServiceCollection services)
    {
        services.AddTransient(typeof(ILlmProviderService), typeof(LlmProviderService));

        services.AddTransient(typeof(IAdminService), typeof(AdminService));

        services.AddTransient<AppInterop, AppInterop>();

        return services;
    }
}
