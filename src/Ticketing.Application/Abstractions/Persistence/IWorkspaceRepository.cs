using Ticketing.Domain.Workspaces;

namespace Ticketing.Application.Abstractions.Persistence;

public interface IWorkspaceRepository
{
    Task AddAsync(Workspace workspace, CancellationToken cancellationToken = default);
    Task<Workspace?> GetByIdAsync(Guid workspaceId, CancellationToken cancellationToken = default);
    Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default);
}
