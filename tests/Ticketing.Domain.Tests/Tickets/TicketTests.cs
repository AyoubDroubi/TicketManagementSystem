using Ticketing.Domain.Common;
using Ticketing.Domain.Tickets;
using Ticketing.Domain.Tickets.Events;
using Xunit;

namespace Ticketing.Domain.Tests.Tickets;

public sealed class TicketTests
{
    private static readonly Guid WorkspaceId = Guid.NewGuid();
    private static readonly Guid RequesterId = Guid.NewGuid();
    private static readonly Guid TeamId = Guid.NewGuid();
    private static readonly DateTimeOffset CreatedAt = new(2026, 8, 13, 7, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Create_InitializesNewTicketAndRaisesCreatedEvent()
    {
        var ticket = CreateTicket();

        Assert.NotEqual(Guid.Empty, ticket.Id);
        Assert.Equal(WorkspaceId, ticket.WorkspaceId);
        Assert.Equal(RequesterId, ticket.RequesterId);
        Assert.Equal("Printer is unavailable", ticket.Summary);
        Assert.Equal(TicketPriority.High, ticket.Priority);
        Assert.Equal(TicketStatus.New, ticket.Status);
        Assert.Equal(CreatedAt, ticket.CreatedAtUtc);
        Assert.Equal(CreatedAt, ticket.UpdatedAtUtc);
        Assert.IsType<TicketCreatedDomainEvent>(Assert.Single(ticket.GetDomainEvents()));
    }

    [Fact]
    public void Assign_FromNew_TransitionsToAssignedAndRecordsStatusEvent()
    {
        var ticket = CreateTicket();
        var assignedAt = CreatedAt.AddMinutes(5);

        ticket.Assign(TeamId, null, assignedAt);

        Assert.Equal(TeamId, ticket.AssignedTeamId);
        Assert.Null(ticket.AssignedAgentId);
        Assert.Equal(TicketStatus.Assigned, ticket.Status);
        Assert.Equal(assignedAt, ticket.UpdatedAtUtc);
        Assert.Contains(ticket.GetDomainEvents(), x => x is TicketStatusChangedDomainEvent statusChanged
            && statusChanged.PreviousStatus == TicketStatus.New
            && statusChanged.CurrentStatus == TicketStatus.Assigned);
    }

    [Fact]
    public void StartProgress_WithoutTeamAssignment_IsRejected()
    {
        var ticket = CreateTicket();

        var exception = Assert.Throws<DomainRuleException>(
            () => ticket.StartProgress(CreatedAt.AddMinutes(5)));

        Assert.Contains("assigned to a team", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(TicketStatus.New, ticket.Status);
    }

    [Fact]
    public void ResolveAndClose_FollowCanonicalLifecycle()
    {
        var ticket = CreateTicket();
        ticket.Assign(TeamId, Guid.NewGuid(), CreatedAt.AddMinutes(5));
        ticket.StartProgress(CreatedAt.AddMinutes(10));

        ticket.Resolve("Replaced the failed printer queue configuration.", CreatedAt.AddHours(1));

        Assert.Equal(TicketStatus.Resolved, ticket.Status);
        Assert.Equal(CreatedAt.AddHours(1), ticket.ResolvedAtUtc);
        Assert.Null(ticket.ClosedAtUtc);

        ticket.Close(CreatedAt.AddHours(2));

        Assert.Equal(TicketStatus.Closed, ticket.Status);
        Assert.Equal(CreatedAt.AddHours(2), ticket.ClosedAtUtc);
    }

    [Fact]
    public void Close_BeforeResolution_IsRejected()
    {
        var ticket = CreateTicket();
        ticket.Assign(TeamId, null, CreatedAt.AddMinutes(5));

        Assert.Throws<DomainRuleException>(() => ticket.Close(CreatedAt.AddMinutes(10)));
        Assert.Equal(TicketStatus.Assigned, ticket.Status);
    }

    [Fact]
    public void Reopen_ClearsCurrentResolutionAndReturnsTicketToWork()
    {
        var ticket = CreateTicket();
        ticket.Assign(TeamId, null, CreatedAt.AddMinutes(5));
        ticket.StartProgress(CreatedAt.AddMinutes(10));
        ticket.Resolve("Temporary workaround applied.", CreatedAt.AddMinutes(30));
        ticket.Close(CreatedAt.AddHours(1));

        ticket.Reopen(CreatedAt.AddHours(2));

        Assert.Equal(TicketStatus.Reopened, ticket.Status);
        Assert.Null(ticket.Resolution);
        Assert.Null(ticket.ResolvedAtUtc);
        Assert.Null(ticket.ClosedAtUtc);
    }

    private static Ticket CreateTicket()
        => Ticket.Create(
            WorkspaceId,
            RequesterId,
            " Printer is unavailable ",
            "The office printer stopped accepting jobs.",
            TicketPriority.High,
            CreatedAt);
}
