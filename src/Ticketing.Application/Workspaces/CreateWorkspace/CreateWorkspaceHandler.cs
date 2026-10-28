using Ticketing.Application.Abstractions;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Common;
using Ticketing.Domain.Workspaces;

namespace Ticketing.Application.Workspaces.CreateWorkspace;

public sealed class CreateWorkspaceHandler
{
    private readonly ICurrentUser _currentUser;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IWorkspaceMembershipRepository _membershipRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IClock _clock;

    public CreateWorkspaceHandler(
        ICurrentUser currentUser,
        IWorkspaceRepository workspaceRepository,
        IWorkspaceMembershipRepository membershipRepository,
        IUnitOfWork unitOfWork,
        IClock clock)
    {
        _currentUser = currentUser;
        _workspaceRepository = workspaceRepository;
        _membershipRepository = membershipRepository;
        _unitOfWork = unitOfWork;
        _clock = clock;
    }

    public async Task<WorkspaceDto> HandleAsync(
        CreateWorkspaceCommand command,
        CancellationToken cancellationToken = default)
    {
        if (!_currentUser.IsAuthenticated)
        {
            throw new DomainRuleException("An authenticated user is required to create a workspace.");
        }

        var slug = command.Slug.Trim().ToLowerInvariant();
        if (await _workspaceRepository.SlugExistsAsync(slug, cancellationToken))
        {
            throw new DomainRuleException("Workspace slug already exists.");
        }

        var workspace = Workspace.Create(command.Name, slug, _clock.UtcNow);
        var membership = WorkspaceMembership.Create(
            workspace.Id,
            _currentUser.UserId,
            WorkspaceMemberType.Owner,
            _clock.UtcNow);

        await _workspaceRepository.AddAsync(workspace, cancellationToken);
        await _membershipRepository.AddAsync(membership, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return WorkspaceDto.FromDomain(workspace);
    }
}
