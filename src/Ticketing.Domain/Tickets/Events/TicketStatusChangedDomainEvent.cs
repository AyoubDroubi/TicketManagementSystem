using Ticketing.Domain.Common;

namespace Ticketing.Domain.Tickets.Events;

public sealed record TicketStatusChangedDomainEvent(
    Guid TicketId,
    Guid WorkspaceId,
    TicketStatus PreviousStatus,
    TicketStatus CurrentStatus,
    DateTimeOffset OccurredAtUtc) : IDomainEvent;
