using Ticketing.Domain.Common;
using Ticketing.Domain.Tickets.Events;

namespace Ticketing.Domain.Tickets;

public sealed class Ticket : Entity<Guid>
{
    public const int MaxSummaryLength = 200;
    public const int MaxDescriptionLength = 20_000;
    public const int MaxResolutionLength = 4_000;

    private Ticket(
        Guid id,
        Guid workspaceId,
        Guid requesterId,
        string summary,
        string? description,
        TicketPriority priority,
        DateTimeOffset createdAtUtc)
        : base(id)
    {
        WorkspaceId = workspaceId;
        RequesterId = requesterId;
        Summary = summary;
        Description = description;
        Priority = priority;
        Status = TicketStatus.New;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    private Ticket() : base(Guid.Empty)
    {
        Summary = string.Empty;
    }

    public Guid WorkspaceId { get; private init; }

    public Guid RequesterId { get; private init; }

    public string Summary { get; private set; }

    public string? Description { get; private set; }

    public TicketPriority Priority { get; private set; }

    public TicketStatus Status { get; private set; }

    public Guid? AssignedTeamId { get; private set; }

    public Guid? AssignedAgentId { get; private set; }

    public string? Resolution { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private init; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public DateTimeOffset? ResolvedAtUtc { get; private set; }

    public DateTimeOffset? ClosedAtUtc { get; private set; }

    public static Ticket Create(
        Guid workspaceId,
        Guid requesterId,
        string summary,
        string? description,
        TicketPriority priority,
        DateTimeOffset createdAtUtc)
    {
        EnsureNotEmpty(workspaceId, nameof(workspaceId));
        EnsureNotEmpty(requesterId, nameof(requesterId));

        var ticket = new Ticket(
            Guid.CreateVersion7(),
            workspaceId,
            requesterId,
            NormalizeRequired(summary, MaxSummaryLength, nameof(summary)),
            NormalizeOptional(description, MaxDescriptionLength, nameof(description)),
            priority,
            createdAtUtc);

        ticket.RaiseDomainEvent(new TicketCreatedDomainEvent(
            ticket.Id,
            ticket.WorkspaceId,
            ticket.RequesterId,
            createdAtUtc));

        return ticket;
    }

    public void UpdateContent(string summary, string? description, DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();

        Summary = NormalizeRequired(summary, MaxSummaryLength, nameof(summary));
        Description = NormalizeOptional(description, MaxDescriptionLength, nameof(description));
        Touch(changedAtUtc);
    }

    public void ChangePriority(TicketPriority priority, DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();

        Priority = priority;
        Touch(changedAtUtc);
    }

    public void MoveToTriage(DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();

        if (Status is not TicketStatus.New and not TicketStatus.Reopened)
        {
            throw new DomainRuleException($"A ticket in '{Status}' cannot move to triage.");
        }

        ChangeStatus(TicketStatus.Triage, changedAtUtc);
    }

    public void Assign(Guid teamId, Guid? agentId, DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();
        EnsureNotEmpty(teamId, nameof(teamId));

        if (agentId == Guid.Empty)
        {
            throw new DomainRuleException("Assigned agent id cannot be an empty GUID.");
        }

        AssignedTeamId = teamId;
        AssignedAgentId = agentId;

        if (Status is TicketStatus.New or TicketStatus.Triage or TicketStatus.Reopened)
        {
            ChangeStatus(TicketStatus.Assigned, changedAtUtc);
            return;
        }

        Touch(changedAtUtc);
    }

    public void StartProgress(DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();

        if (AssignedTeamId is null)
        {
            throw new DomainRuleException("A ticket must be assigned to a team before work starts.");
        }

        if (Status is not TicketStatus.Assigned
            and not TicketStatus.Reopened
            and not TicketStatus.PendingRequester
            and not TicketStatus.PendingInternal
            and not TicketStatus.PendingThirdParty)
        {
            throw new DomainRuleException($"A ticket in '{Status}' cannot start progress.");
        }

        ChangeStatus(TicketStatus.InProgress, changedAtUtc);
    }

    public void WaitForRequester(DateTimeOffset changedAtUtc)
        => MoveToPending(TicketStatus.PendingRequester, changedAtUtc);

    public void WaitForInternal(DateTimeOffset changedAtUtc)
        => MoveToPending(TicketStatus.PendingInternal, changedAtUtc);

    public void WaitForThirdParty(DateTimeOffset changedAtUtc)
        => MoveToPending(TicketStatus.PendingThirdParty, changedAtUtc);

    public void Resolve(string resolution, DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();

        if (Status is TicketStatus.New or TicketStatus.Triage)
        {
            throw new DomainRuleException("A ticket must be assigned and worked before it can be resolved.");
        }

        Resolution = NormalizeRequired(resolution, MaxResolutionLength, nameof(resolution));
        ResolvedAtUtc = changedAtUtc;
        ClosedAtUtc = null;
        ChangeStatus(TicketStatus.Resolved, changedAtUtc);
    }

    public void Close(DateTimeOffset changedAtUtc)
    {
        if (Status != TicketStatus.Resolved)
        {
            throw new DomainRuleException("Only a resolved ticket can be closed.");
        }

        ClosedAtUtc = changedAtUtc;
        ChangeStatus(TicketStatus.Closed, changedAtUtc);
    }

    public void Reopen(DateTimeOffset changedAtUtc)
    {
        if (Status is not TicketStatus.Resolved and not TicketStatus.Closed)
        {
            throw new DomainRuleException("Only a resolved or closed ticket can be reopened.");
        }

        Resolution = null;
        ResolvedAtUtc = null;
        ClosedAtUtc = null;
        ChangeStatus(TicketStatus.Reopened, changedAtUtc);
    }

    public void Cancel(DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();

        Resolution = null;
        ResolvedAtUtc = null;
        ClosedAtUtc = null;
        ChangeStatus(TicketStatus.Cancelled, changedAtUtc);
    }

    private void MoveToPending(TicketStatus pendingStatus, DateTimeOffset changedAtUtc)
    {
        EnsureOpenForWork();

        if (Status is not TicketStatus.InProgress and not TicketStatus.Assigned)
        {
            throw new DomainRuleException($"A ticket in '{Status}' cannot move to '{pendingStatus}'.");
        }

        ChangeStatus(pendingStatus, changedAtUtc);
    }

    private void EnsureOpenForWork()
    {
        if (Status is TicketStatus.Resolved
            or TicketStatus.Closed
            or TicketStatus.Cancelled
            or TicketStatus.Duplicate)
        {
            throw new DomainRuleException($"A ticket in '{Status}' cannot be modified as active work.");
        }
    }

    private void ChangeStatus(TicketStatus newStatus, DateTimeOffset changedAtUtc)
    {
        if (Status == newStatus)
        {
            Touch(changedAtUtc);
            return;
        }

        var previousStatus = Status;
        Status = newStatus;
        Touch(changedAtUtc);

        RaiseDomainEvent(new TicketStatusChangedDomainEvent(
            Id,
            WorkspaceId,
            previousStatus,
            newStatus,
            changedAtUtc));
    }

    private void Touch(DateTimeOffset changedAtUtc)
    {
        if (changedAtUtc < CreatedAtUtc)
        {
            throw new DomainRuleException("A ticket change timestamp cannot be earlier than its creation timestamp.");
        }

        UpdatedAtUtc = changedAtUtc;
    }

    private static void EnsureNotEmpty(Guid value, string parameterName)
    {
        if (value == Guid.Empty)
        {
            throw new DomainRuleException($"{parameterName} cannot be an empty GUID.");
        }
    }

    private static string NormalizeRequired(string value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException($"{parameterName} is required.");
        }

        var normalized = value.Trim();

        if (normalized.Length > maxLength)
        {
            throw new DomainRuleException($"{parameterName} cannot exceed {maxLength} characters.");
        }

        return normalized;
    }

    private static string? NormalizeOptional(string? value, int maxLength, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var normalized = value.Trim();

        if (normalized.Length > maxLength)
        {
            throw new DomainRuleException($"{parameterName} cannot exceed {maxLength} characters.");
        }

        return normalized;
    }
}
