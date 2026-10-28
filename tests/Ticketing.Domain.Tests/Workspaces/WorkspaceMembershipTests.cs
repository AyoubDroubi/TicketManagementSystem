using Ticketing.Domain.Common;
using Ticketing.Domain.Workspaces;
using Xunit;

namespace Ticketing.Domain.Tests.Workspaces;

public sealed class WorkspaceMembershipTests
{
    private static readonly DateTimeOffset CreatedAt = new(2026, 8, 13, 20, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Create_StartsActive()
    {
        var membership = WorkspaceMembership.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            WorkspaceMemberType.Agent,
            CreatedAt);

        Assert.Equal(WorkspaceMemberType.Agent, membership.MemberType);
        Assert.Equal(MembershipState.Active, membership.State);
    }

    [Fact]
    public void DisabledMembership_CannotChangeType()
    {
        var membership = WorkspaceMembership.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            WorkspaceMemberType.Agent,
            CreatedAt);

        membership.Disable(CreatedAt.AddMinutes(5));

        Assert.Throws<DomainRuleException>(() =>
            membership.ChangeMemberType(WorkspaceMemberType.Manager, CreatedAt.AddMinutes(10)));
    }

    [Fact]
    public void OwnerMembership_CannotBeDisabled()
    {
        var membership = WorkspaceMembership.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            WorkspaceMemberType.Owner,
            CreatedAt);

        Assert.Throws<DomainRuleException>(() =>
            membership.Disable(CreatedAt.AddMinutes(5)));
    }
}
