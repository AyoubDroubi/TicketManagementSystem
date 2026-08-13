using Ticketing.Domain.Tickets;

namespace Ticketing.Application.Tickets.CreateTicket;

public sealed record CreateTicketCommand(
    string Summary,
    string? Description,
    TicketPriority Priority);
