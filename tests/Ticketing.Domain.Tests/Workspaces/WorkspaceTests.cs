using Ticketing.Domain.Common;
using Ticketing.Domain.Workspaces;
using Xunit;

namespace Ticketing.Domain.Tests.Workspaces;

public sealed class WorkspaceTests
{
    private static readonly DateTimeOffset CreatedAt = new(2026, 8, 13, 18, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Create_NormalizesIdentityAndStartsActive()
    {
        var workspace = Workspace.Create("  Acme Support  ", "  ACME-SUPPORT  ", CreatedAt);

        Assert.NotEqual(Guid.Empty, workspace.Id);
        Assert.Equal("Acme Support", workspace.Name);
        Assert.Equal("acme-support", workspace.Slug);
        Assert.Equal(WorkspaceStatus.Active, workspace.Status);
        Assert.Equal(CreatedAt, workspace.CreatedAtUtc);
    }

    [Fact]
    public void Create_WithInvalidSlug_IsRejected()
    {
        Assert.Throws<DomainRuleException>(() =>
            Workspace.Create("Acme", "acme_support", CreatedAt));
    }

    [Fact]
    public void ArchivedWorkspace_CannotBeReactivated()
    {
        var workspace = Workspace.Create("Acme", "acme", CreatedAt);
        workspace.Archive(CreatedAt.AddMinutes(10));

        Assert.Throws<DomainRuleException>(() =>
            workspace.Activate(CreatedAt.AddMinutes(20)));
        Assert.Equal(WorkspaceStatus.Archived, workspace.Status);
    }
}
