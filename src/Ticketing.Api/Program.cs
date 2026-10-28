using Ticketing.Api;
using Ticketing.Api.Accounts;
using Ticketing.Infrastructure;
using Ticketing.Infrastructure.Accounts;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddProblemDetails();
builder.Services.AddApiV2();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAccountPersistence(builder.Configuration);
builder.Services.AddAccountApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapDefaultEndpoints();
app.MapAccountApi();
app.MapV2Endpoints();

app.Run();

public partial class Program;
