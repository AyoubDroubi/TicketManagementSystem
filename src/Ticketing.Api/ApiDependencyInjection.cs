using System.Text.Json.Serialization;
using Ticketing.Api.Tenancy;
using Ticketing.Application.Abstractions;

namespace Ticketing.Api;

internal static class ApiDependencyInjection
{
    public static IServiceCollection AddApiV2(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IWorkspaceContext, HttpWorkspaceContext>();
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        return services;
    }
}
