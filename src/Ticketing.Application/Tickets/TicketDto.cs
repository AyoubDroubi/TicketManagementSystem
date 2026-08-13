using Ticketing.Domain.Tickets;

namespace Ticketing.Application.Tickets;

public sealed record TicketDto(
    Guid Id,
    Guid WorkspaceId,
    Guid RequesterId,
    string Summary,
    string? Description,
    TicketPriority Priority,
    TicketStatus Status,
    Guid? AssignedTeamId,
    Guid? AssignedAgentId,
    string? Resolution,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc,
    DateTimeOffset? ResolvedAtUtc,
    DateTimeOffset? ClosedAtUtc)
{
    public static TicketDto FromDomain(Ticket ticket)
        => new(
            ticket.Id,
            ticket.WorkspaceId,
            ticket.RequesterId,
            ticket.Summary,
            ticket.Description,
            ticket.Priority,
            ticket.Status,
            ticket.AssignedTeamId,
            ticket.AssignedAgentId,
            ticket.Resolution,
            ticket.CreatedAtUtc,
            ticket.UpdatedAtUtc,
            ticket.ResolvedAtUtc,
            ticket.ClosedAtUtc);
}
