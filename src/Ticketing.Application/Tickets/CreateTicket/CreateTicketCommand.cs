using Ticketing.Domain.Tickets;

namespace Ticketing.Application.Tickets.CreateTicket;

public sealed record CreateTicketCommand(
    Guid RequesterId,
    string Summary,
    string? Description,
    TicketPriority Priority);
