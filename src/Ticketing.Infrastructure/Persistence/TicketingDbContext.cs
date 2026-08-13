using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Abstractions;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Domain.Tickets;
using Ticketing.Domain.Workspaces;

namespace Ticketing.Infrastructure.Persistence;

public sealed class TicketingDbContext : DbContext, IUnitOfWork
{
    private readonly IWorkspaceContext _workspaceContext;

    public TicketingDbContext(
        DbContextOptions<TicketingDbContext> options,
        IWorkspaceContext workspaceContext)
        : base(options)
    {
        _workspaceContext = workspaceContext;
    }

    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Workspace> Workspaces => Set<Workspace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TicketingDbContext).Assembly);
        modelBuilder.Entity<Ticket>()
            .HasQueryFilter(ticket => ticket.WorkspaceId == _workspaceContext.WorkspaceId);
    }
}
