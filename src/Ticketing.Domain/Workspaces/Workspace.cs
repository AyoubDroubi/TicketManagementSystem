using Ticketing.Domain.Common;

namespace Ticketing.Domain.Workspaces;

public sealed class Workspace : Entity<Guid>
{
    public const int MaxNameLength = 120;
    public const int MaxSlugLength = 80;

    private Workspace(Guid id, string name, string slug, DateTimeOffset createdAtUtc)
        : base(id)
    {
        Name = name;
        Slug = slug;
        Status = WorkspaceStatus.Active;
        CreatedAtUtc = createdAtUtc;
        UpdatedAtUtc = createdAtUtc;
    }

    private Workspace() : base(Guid.Empty)
    {
        Name = string.Empty;
        Slug = string.Empty;
    }

    public string Name { get; private set; }

    public string Slug { get; private set; }

    public WorkspaceStatus Status { get; private set; }

    public DateTimeOffset CreatedAtUtc { get; private init; }

    public DateTimeOffset UpdatedAtUtc { get; private set; }

    public static Workspace Create(string name, string slug, DateTimeOffset createdAtUtc)
    {
        return new Workspace(
            Guid.CreateVersion7(),
            NormalizeName(name),
            NormalizeSlug(slug),
            createdAtUtc);
    }

    public void Rename(string name, DateTimeOffset changedAtUtc)
    {
        Name = NormalizeName(name);
        Touch(changedAtUtc);
    }

    public void Deactivate(DateTimeOffset changedAtUtc)
    {
        Status = WorkspaceStatus.Inactive;
        Touch(changedAtUtc);
    }

    public void Activate(DateTimeOffset changedAtUtc)
    {
        if (Status == WorkspaceStatus.Archived)
        {
            throw new DomainRuleException("An archived workspace cannot be reactivated.");
        }

        Status = WorkspaceStatus.Active;
        Touch(changedAtUtc);
    }

    public void Archive(DateTimeOffset changedAtUtc)
    {
        Status = WorkspaceStatus.Archived;
        Touch(changedAtUtc);
    }

    private void Touch(DateTimeOffset changedAtUtc)
    {
        if (changedAtUtc < CreatedAtUtc)
        {
            throw new DomainRuleException("A workspace change timestamp cannot be earlier than its creation timestamp.");
        }

        UpdatedAtUtc = changedAtUtc;
    }

    private static string NormalizeName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException("Workspace name is required.");
        }

        var normalized = value.Trim();
        if (normalized.Length > MaxNameLength)
        {
            throw new DomainRuleException($"Workspace name cannot exceed {MaxNameLength} characters.");
        }

        return normalized;
    }

    private static string NormalizeSlug(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainRuleException("Workspace slug is required.");
        }

        var normalized = value.Trim().ToLowerInvariant();
        if (normalized.Length > MaxSlugLength)
        {
            throw new DomainRuleException($"Workspace slug cannot exceed {MaxSlugLength} characters.");
        }

        if (normalized.Any(character => !char.IsLetterOrDigit(character) && character != '-'))
        {
            throw new DomainRuleException("Workspace slug may contain only letters, numbers, and hyphens.");
        }

        return normalized;
    }
}
