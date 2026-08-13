using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Abstractions;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Infrastructure.Persistence;
using Ticketing.Infrastructure.Time;

namespace Ticketing.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TicketingDatabase")
            ?? throw new InvalidOperationException(
                "Connection string 'TicketingDatabase' is required. Configure it outside source control.");

        services.AddDbContext<TicketingDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                npgsql.EnableRetryOnFailure(maxRetryCount: 5)));

        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IWorkspaceRepository, WorkspaceRepository>();
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<TicketingDbContext>());
        services.AddSingleton<IClock, SystemClock>();

        return services;
    }
}
