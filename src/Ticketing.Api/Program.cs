using Ticketing.Api;
using Ticketing.Application.Abstractions;
using Ticketing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddApiV2();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapGet("/api/v2/system", (IClock clock) => Results.Ok(new
{
    service = "ticketing-api",
    version = "v2",
    utcNow = clock.UtcNow
}));

app.Run();

public partial class Program;
