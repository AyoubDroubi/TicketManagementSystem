using System.Text.Json.Serialization;
using Ticketing.Api.Errors;
using Ticketing.Api.Tenancy;
using Ticketing.Application.Abstractions;
using Ticketing.Application.Tickets.CreateTicket;
using Ticketing.Application.Tickets.GetTicketById;
using Ticketing.Application.Workspaces.CreateWorkspace;

namespace Ticketing.Api;

internal static class ApiDependencyInjection
{
    public static IServiceCollection AddApiV2(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddExceptionHandler<BusinessRuleExceptionHandler>();
        services.AddScoped<IWorkspaceContext, HttpWorkspaceContext>();
        services.AddScoped<CreateWorkspaceHandler>();
        services.AddScoped<CreateTicketHandler>();
        services.AddScoped<GetTicketByIdHandler>();
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        return services;
    }
}
