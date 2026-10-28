using Ticketing.Domain.Workspaces;

namespace Ticketing.Application.Workspaces;

public sealed record WorkspaceDto(
    Guid Id,
    string Name,
    string Slug,
    WorkspaceStatus Status,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset UpdatedAtUtc)
{
    public static WorkspaceDto FromDomain(Workspace workspace)
        => new(
            workspace.Id,
            workspace.Name,
            workspace.Slug,
            workspace.Status,
            workspace.CreatedAtUtc,
            workspace.UpdatedAtUtc);
}
