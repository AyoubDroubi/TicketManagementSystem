var builder = DistributedApplication.CreateBuilder(args);

var database = builder
    .AddPostgres("postgres")
    .WithDataVolume("ticketing-postgres")
    .AddDatabase("TicketingDatabase", "ticketing");

builder.AddProject<Projects.Ticketing_Api>("api")
    .WithReference(database)
    .WaitFor(database)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
