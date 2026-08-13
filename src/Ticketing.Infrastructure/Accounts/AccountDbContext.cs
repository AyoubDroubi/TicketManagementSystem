using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Infrastructure.Accounts;

public sealed class AccountDbContext(DbContextOptions<AccountDbContext> options)
    : IdentityDbContext(options)
{
}
