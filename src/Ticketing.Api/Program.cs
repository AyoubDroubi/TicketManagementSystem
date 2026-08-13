using Ticketing.Application;
using Ticketing.Application.Abstractions;
using Ticketing.Infrastructure;
using Ticketing.Infrastructure.Time;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddApplication();
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
