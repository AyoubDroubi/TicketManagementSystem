using Ticketing.Domain.Common;

namespace Ticketing.Domain.Workspaces;

public sealed class WorkspaceMembership : Entity<Guid>
{
    private WorkspaceMembership(
        Guid id,
        Guid workspaceId,
        Guid userId,
        WorkspaceMemberType memberType,
        DateTimeOffset createdAtUtc)
        : base(id)
    {
        WorkspaceId = workspaceId;
        UserId = userId;
        MemberType = memberType;
        State = MembershipState.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    private WorkspaceMembership() : base(Guid.Empty)
    {
    }

    public Guid WorkspaceId { get; private init; }
    public Guid UserId { get; private init; }
    public WorkspaceMemberType MemberType { get; private set; }
    public MembershipState State { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private init; }
    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static WorkspaceMembership Create(
        Guid workspaceId,
        Guid userId,
        WorkspaceMemberType memberType,
        DateTimeOffset createdAtUtc)
    {
        if (workspaceId == Guid.Empty || userId == Guid.Empty)
        {
            throw new DomainRuleException("Workspace membership requires valid workspace and user ids.");
        }

        return new WorkspaceMembership(
            Guid.CreateVersion7(),
            workspaceId,
            userId,
            memberType,
            createdAtUtc);
    }

    public void ChangeMemberType(WorkspaceMemberType memberType, DateTimeOffset changedAtUtc)
    {
        EnsureActive();
        MemberType = memberType;
        Touch(changedAtUtc);
    }

    public void Disable(DateTimeOffset changedAtUtc)
    {
        if (MemberType == WorkspaceMemberType.Owner)
        {
            throw new DomainRuleException("The workspace owner membership cannot be disabled.");
        }

        State = MembershipState.Disabled;
        Touch(changedAtUtc);
    }

    public void Enable(DateTimeOffset changedAtUtc)
    {
        State = MembershipState.Active;
        Touch(changedAtUtc);
    }

    private void EnsureActive()
    {
        if (State != MembershipState.Active)
        {
            throw new DomainRuleException("A disabled workspace membership cannot be modified.");
        }
    }

    private void Touch(DateTimeOffset changedAtUtc)
    {
        if (changedAtUtc < CreatedAtUtc)
        {
            throw new DomainRuleException("A membership change timestamp cannot be earlier than its creation timestamp.");
        }

        UpdatedAtUtc = changedAtUtc;
    }
}
