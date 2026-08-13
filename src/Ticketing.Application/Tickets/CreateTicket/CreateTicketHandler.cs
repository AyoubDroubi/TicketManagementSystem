using Ticketing.Application.Abstractions;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Common;
using Ticketing.Domain.Tickets;
using Ticketing.Domain.Workspaces;

namespace Ticketing.Application.Tickets.CreateTicket;

public sealed class CreateTicketHandler
{
    private readonly IWorkspaceContext _workspaceContext;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;

    public CreateTicketHandler(
        IWorkspaceContext workspaceContext,
        IWorkspaceRepository workspaceRepository,
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        _workspaceContext = workspaceContext;
        _workspaceRepository = workspaceRepository;
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<TicketDto> HandleAsync(
        CreateTicketCommand command,
        CancellationToken cancellationToken = default)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(
            _workspaceContext.WorkspaceId,
            cancellationToken);

        if (workspace is null || workspace.Status != WorkspaceStatus.Active)
        {
            throw new DomainRuleException("An active workspace is required.");
        }

        var item = Ticket.Create(
            workspace.Id,
            command.RequesterId,
            command.Summary,
            command.Description,
            command.Priority,
            _clock.UtcNow);

        await _ticketRepository.AddAsync(item, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return TicketDto.FromDomain(item);
    }
}
