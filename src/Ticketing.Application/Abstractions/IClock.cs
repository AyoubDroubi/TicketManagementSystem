namespace Ticketing.Application.Abstractions;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
