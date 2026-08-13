using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ticketing.Infrastructure.Accounts;

public static class AccountPersistence
{
    public static IServiceCollection AddAccountPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TicketingDatabase")
            ?? throw new InvalidOperationException("Connection string 'TicketingDatabase' is required.");

        services.AddDbContext<AccountDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}
