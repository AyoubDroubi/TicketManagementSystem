using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Workspaces;

namespace Ticketing.Infrastructure.Persistence;

internal sealed class WorkspaceMap : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("workspaces");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedNever();
        builder.Property(item => item.Name).HasMaxLength(Workspace.MaxNameLength).IsRequired();
        builder.Property(item => item.Slug).HasMaxLength(Workspace.MaxSlugLength).IsRequired();
        builder.Property(item => item.Status).HasConversion<string>().HasMaxLength(24).IsRequired();
    }
}
