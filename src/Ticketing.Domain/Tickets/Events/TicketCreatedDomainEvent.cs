using Ticketing.Domain.Common;

namespace Ticketing.Domain.Tickets.Events;

public sealed record TicketCreatedDomainEvent(
    Guid TicketId,
    Guid WorkspaceId,
    Guid RequesterId,
    DateTimeOffset OccurredAtUtc) : IDomainEvent;
