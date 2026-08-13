namespace Ticketing.Domain.Tickets;

public enum TicketStatus
{
    New = 1,
    Triage = 2,
    Assigned = 3,
    InProgress = 4,
    PendingRequester = 5,
    PendingInternal = 6,
    PendingThirdParty = 7,
    Resolved = 8,
    Closed = 9,
    Reopened = 10,
    Cancelled = 11,
    Duplicate = 12
}
