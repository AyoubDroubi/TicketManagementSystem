using Ticketing.Api;
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
app.MapV2Endpoints();

app.Run();

public partial class Program;
