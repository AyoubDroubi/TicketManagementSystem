using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Tickets;
using Ticketing.Domain.Workspaces;

namespace Ticketing.Infrastructure.Persistence;

internal static class PersistenceModel
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        var tickets = modelBuilder.Entity<Ticket>();
        tickets.ToTable("tickets");
        tickets.HasKey(item => item.Id);
        tickets.Property(item => item.Id).ValueGeneratedNever();
        tickets.Property(item => item.WorkspaceId).IsRequired();
        tickets.Property(item => item.RequesterId).IsRequired();
        tickets.Property(item => item.Summary).HasMaxLength(Ticket.MaxSummaryLength).IsRequired();
        tickets.Property(item => item.Description).HasMaxLength(Ticket.MaxDescriptionLength);
        tickets.Property(item => item.Resolution).HasMaxLength(Ticket.MaxResolutionLength);
        tickets.Property(item => item.Priority).HasConversion<string>().HasMaxLength(24).IsRequired();
        tickets.Property(item => item.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
        tickets.Property(item => item.CreatedAtUtc).IsRequired();
        tickets.Property(item => item.UpdatedAtUtc).IsRequired();
        tickets.HasIndex(item => new { item.WorkspaceId, item.Status, item.CreatedAtUtc });
        tickets.HasIndex(item => new { item.WorkspaceId, item.Priority, item.CreatedAtUtc });
        tickets.HasIndex(item => new { item.WorkspaceId, item.RequesterId, item.CreatedAtUtc });

        var workspaces = modelBuilder.Entity<Workspace>();
        workspaces.ToTable("workspaces");
        workspaces.HasKey(item => item.Id);
        workspaces.Property(item => item.Id).ValueGeneratedNever();
        workspaces.Property(item => item.Name).HasMaxLength(Workspace.MaxNameLength).IsRequired();
        workspaces.Property(item => item.Slug).HasMaxLength(Workspace.MaxSlugLength).IsRequired();
        workspaces.Property(item => item.Status).HasConversion<string>().HasMaxLength(24).IsRequired();
        workspaces.HasIndex(item => item.Slug).IsUnique();
    }
}
