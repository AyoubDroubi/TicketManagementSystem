using Ticketing.Application.Abstractions;
using Ticketing.Application.Tickets.CreateTicket;
using Ticketing.Application.Tickets.GetTicketById;
using Ticketing.Application.Workspaces.CreateWorkspace;

namespace Ticketing.Api;

internal static class V2Endpoints
{
    public static IEndpointRouteBuilder MapV2Endpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/v2/system", (IClock clock) => Results.Ok(new
        {
            service = "ticketing-api",
            version = "v2",
            utcNow = clock.UtcNow
        }));

        endpoints.MapPost("/api/v2/workspaces", async (
            CreateWorkspaceCommand command,
            CreateWorkspaceHandler handler,
            CancellationToken cancellationToken) =>
        {
            var workspace = await handler.HandleAsync(command, cancellationToken);
            return Results.Created($"/api/v2/workspaces/{workspace.Id}", workspace);
        }).RequireAuthorization();

        endpoints.MapPost("/api/v2/tickets", async (
            CreateTicketCommand command,
            CreateTicketHandler handler,
            CancellationToken cancellationToken) =>
        {
            var ticket = await handler.HandleAsync(command, cancellationToken);
            return Results.Created($"/api/v2/tickets/{ticket.Id}", ticket);
        }).RequireAuthorization();

        endpoints.MapGet("/api/v2/tickets/{ticketId:guid}", async (
            Guid ticketId,
            GetTicketByIdHandler handler,
            CancellationToken cancellationToken) =>
        {
            var ticket = await handler.HandleAsync(new GetTicketByIdQuery(ticketId), cancellationToken);
            return ticket is null ? Results.NotFound() : Results.Ok(ticket);
        }).RequireAuthorization();

        return endpoints;
    }
}
