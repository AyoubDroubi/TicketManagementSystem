using Ticketing.Domain.Tickets;
using Xunit;

namespace Ticketing.Domain.Tests.Architecture;

public sealed class DomainDependencyTests
{
    private static readonly string[] ForbiddenReferencePrefixes =
    [
        "Microsoft.EntityFrameworkCore",
        "Microsoft.AspNetCore",
        "Microsoft.Extensions.DependencyInjection",
        "Npgsql",
        "StackExchange.Redis"
    ];

    [Fact]
    public void DomainAssembly_DoesNotReferenceInfrastructureOrWebFrameworks()
    {
        var referencedAssemblies = typeof(Ticket).Assembly
            .GetReferencedAssemblies()
            .Select(reference => reference.Name ?? string.Empty)
            .ToArray();

        foreach (var forbiddenPrefix in ForbiddenReferencePrefixes)
        {
            Assert.DoesNotContain(
                referencedAssemblies,
                assemblyName => assemblyName.StartsWith(forbiddenPrefix, StringComparison.Ordinal));
        }
    }
}
