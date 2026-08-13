using Ticketing.Api;
using Ticketing.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddApiV2();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapDefaultEndpoints();
app.MapV2Endpoints();

app.Run();

public partial class Program;
