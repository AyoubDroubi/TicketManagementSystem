using Microsoft.AspNetCore.Identity;
using Ticketing.Application.Abstractions;
using Ticketing.Infrastructure.Accounts;

namespace Ticketing.Api.Accounts;

internal static class AccountApi
{
    public static IServiceCollection AddAccountApi(this IServiceCollection services)
    {
        services.AddAuthorization();
        services.AddScoped<ICurrentUser, RequestUserContext>();

        services.AddIdentityApiEndpoints<IdentityUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 10;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        })
        .AddEntityFrameworkStores<AccountDbContext>();

        return services;
    }

    public static IEndpointRouteBuilder MapAccountApi(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/api/v2/auth")
            .MapIdentityApi<IdentityUser>();

        return endpoints;
    }
}
