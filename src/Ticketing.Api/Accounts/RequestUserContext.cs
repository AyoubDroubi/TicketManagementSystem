using System.Security.Claims;
using Ticketing.Application.Abstractions;

namespace Ticketing.Api.Accounts;

internal sealed class RequestUserContext(IHttpContextAccessor accessor) : ICurrentUser
{
    public bool IsAuthenticated => accessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Guid UserId
    {
        get
        {
            var value = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(value, out var id)
                ? id
                : throw new InvalidOperationException("User context is unavailable.");
        }
    }
}
