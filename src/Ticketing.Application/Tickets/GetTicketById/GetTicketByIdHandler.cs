using Ticketing.Application.Abstractions.Persistence;

namespace Ticketing.Application.Tickets.GetTicketById;

public sealed class GetTicketByIdHandler(ITicketRepository ticketRepository)
{
    public async Task<TicketDto?> HandleAsync(
        GetTicketByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var ticket = await ticketRepository.GetByIdAsync(query.TicketId, cancellationToken);
        return ticket is null ? null : TicketDto.FromDomain(ticket);
    }
}
