using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Workspaces;

namespace Ticketing.Infrastructure.Persistence;

internal sealed class WorkspaceRepository(TicketingDbContext dbContext) : IWorkspaceRepository
{
    public async Task AddAsync(Workspace workspace, CancellationToken cancellationToken = default)
        => await dbContext.Workspaces.AddAsync(workspace, cancellationToken);

    public Task<Workspace?> GetByIdAsync(Guid workspaceId, CancellationToken cancellationToken = default)
        => dbContext.Workspaces.SingleOrDefaultAsync(workspace => workspace.Id == workspaceId, cancellationToken);

    public Task<bool> SlugExistsAsync(string slug, CancellationToken cancellationToken = default)
        => dbContext.Workspaces.AnyAsync(workspace => workspace.Slug == slug, cancellationToken);
}
