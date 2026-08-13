using Ticketing.Application.Abstractions;

namespace Ticketing.Api.Tenancy;

internal sealed class HttpWorkspaceContext(IHttpContextAccessor httpContextAccessor) : IWorkspaceContext
{
    public Guid WorkspaceId
    {
        get
        {
            var httpContext = httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("Workspace context requires an active HTTP request.");

            var rawValue = httpContext.Request.Headers["X-Workspace-Id"].FirstOrDefault();
            if (!Guid.TryParse(rawValue, out var workspaceId) || workspaceId == Guid.Empty)
            {
                throw new BadHttpRequestException("A valid X-Workspace-Id header is required.");
            }

            return workspaceId;
        }
    }
}
