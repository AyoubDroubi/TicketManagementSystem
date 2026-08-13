using Ticketing.Application.Abstractions;
using Ticketing.Infrastructure.Time;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IClock, SystemClock>();

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
