using Ticketing.Domain.Workspaces;

namespace Ticketing.Application.Abstractions.Persistence;

public interface IWorkspaceMembershipRepository
{
    Task AddAsync(WorkspaceMembership membership, CancellationToken cancellationToken = default);
    Task<WorkspaceMembership?> GetAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
    Task<bool> HasActiveMembershipAsync(Guid workspaceId, Guid userId, CancellationToken cancellationToken = default);
}
