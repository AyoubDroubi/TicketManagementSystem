using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Tickets.CreateTicket;
using Ticketing.Application.Tickets.GetTicketById;
using Ticketing.Application.Workspaces.CreateWorkspace;

namespace Ticketing.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateWorkspaceHandler>();
        services.AddScoped<CreateTicketHandler>();
        services.AddScoped<GetTicketByIdHandler>();
        return services;
    }
}
