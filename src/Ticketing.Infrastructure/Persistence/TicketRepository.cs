using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Tickets;

namespace Ticketing.Infrastructure.Persistence;

internal sealed class TicketRepository(TicketingDbContext dbContext) : ITicketRepository
{
    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
        => await dbContext.Tickets.AddAsync(ticket, cancellationToken);

    public Task<Ticket?> GetByIdAsync(Guid ticketId, CancellationToken cancellationToken = default)
        => dbContext.Tickets.SingleOrDefaultAsync(ticket => ticket.Id == ticketId, cancellationToken);
}
