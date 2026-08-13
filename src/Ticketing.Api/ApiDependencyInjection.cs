using Ticketing.Api.Tenancy;
using Ticketing.Application.Abstractions;

namespace Ticketing.Api;

internal static class ApiDependencyInjection
{
    public static IServiceCollection AddApiV2(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IWorkspaceContext, HttpWorkspaceContext>();
        return services;
    }
}
