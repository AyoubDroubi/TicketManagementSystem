using Ticketing.Domain.Tickets;

namespace Ticketing.Application.Abstractions.Persistence;

public interface ITicketRepository
{
    Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default);
    Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default);
}
