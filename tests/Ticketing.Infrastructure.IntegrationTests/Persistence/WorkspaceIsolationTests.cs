using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Abstractions;
using Ticketing.Domain.Tickets;
using Ticketing.Domain.Workspaces;
using Ticketing.Infrastructure.Persistence;
using Xunit;

namespace Ticketing.Infrastructure.IntegrationTests.Persistence;

public sealed class WorkspaceIsolationTests
{
    [Fact]
    public async Task TicketQueries_AreAutomaticallyScopedToCurrentWorkspace()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        var connectionString = Environment.GetEnvironmentVariable("TEST_POSTGRES_CONNECTION_STRING");
        Assert.False(string.IsNullOrWhiteSpace(connectionString),
            "TEST_POSTGRES_CONNECTION_STRING must point to a disposable PostgreSQL test database.");

        var now = new DateTimeOffset(2026, 8, 13, 18, 30, 0, TimeSpan.Zero);
        var workspaceA = Workspace.Create("Alpha Support", "alpha-support", now);
        var workspaceB = Workspace.Create("Beta Support", "beta-support", now);

        await using (var setup = CreateContext(connectionString, workspaceA.Id))
        {
            await setup.Database.EnsureDeletedAsync(cancellationToken);
            await setup.Database.MigrateAsync(cancellationToken);
            setup.Workspaces.AddRange(workspaceA, workspaceB);
            setup.Tickets.Add(Ticket.Create(workspaceA.Id, Guid.CreateVersion7(), "Alpha ticket", null, TicketPriority.High, now));
            setup.Tickets.Add(Ticket.Create(workspaceB.Id, Guid.CreateVersion7(), "Beta ticket", null, TicketPriority.Normal, now));
            await setup.SaveChangesAsync(cancellationToken);
        }

        await using var alphaContext = CreateContext(connectionString, workspaceA.Id);
        await using var betaContext = CreateContext(connectionString, workspaceB.Id);
        var alphaTickets = await alphaContext.Tickets.AsNoTracking().ToListAsync(cancellationToken);
        var betaTickets = await betaContext.Tickets.AsNoTracking().ToListAsync(cancellationToken);

        Assert.Single(alphaTickets);
        Assert.Equal("Alpha ticket", alphaTickets[0].Summary);
        Assert.Equal(workspaceA.Id, alphaTickets[0].WorkspaceId);
        Assert.Single(betaTickets);
        Assert.Equal("Beta ticket", betaTickets[0].Summary);
        Assert.Equal(workspaceB.Id, betaTickets[0].WorkspaceId);
    }

    private static TicketingDbContext CreateContext(string connectionString, Guid workspaceId)
    {
        var options = new DbContextOptionsBuilder<TicketingDbContext>()
            .UseNpgsql(connectionString)
            .Options;
        return new TicketingDbContext(options, new TestWorkspaceContext(workspaceId));
    }

    private sealed class TestWorkspaceContext(Guid workspaceId) : IWorkspaceContext
    {
        public Guid WorkspaceId { get; } = workspaceId;
    }
}
