using Ticketing.Application.Abstractions;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Common;
using Ticketing.Domain.Workspaces;

namespace Ticketing.Application.Workspaces.CreateWorkspace;

public sealed class CreateWorkspaceHandler
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;

    public CreateWorkspaceHandler(
        IWorkspaceRepository workspaceRepository,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        _workspaceRepository = workspaceRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<WorkspaceDto> HandleAsync(
        CreateWorkspaceCommand command,
        CancellationToken cancellationToken = default)
    {
        var slug = command.Slug.Trim().ToLowerInvariant();
        if (await _workspaceRepository.SlugExistsAsync(slug, cancellationToken))
        {
            throw new DomainRuleException("Workspace slug already exists.");
        }

        var workspace = Workspace.Create(command.Name, slug, _clock.UtcNow);
        await _workspaceRepository.AddAsync(workspace, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return WorkspaceDto.FromDomain(workspace);
    }
}
