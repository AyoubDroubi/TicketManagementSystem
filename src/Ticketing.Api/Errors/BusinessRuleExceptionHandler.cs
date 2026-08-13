using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ticketing.Domain.Common;

namespace Ticketing.Api.Errors;

internal sealed class BusinessRuleExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not DomainRuleException domainException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;

        var details = new ProblemDetails
        {
            Status = StatusCodes.Status422UnprocessableEntity,
            Title = "Business rule violation",
            Detail = domainException.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(details, cancellationToken);
        return true;
    }
}
