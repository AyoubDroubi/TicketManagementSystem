namespace Ticketing.Domain.Common;

public interface IDomainEvent
{
    DateTimeOffset OccurredAtUtc { get; }
}
