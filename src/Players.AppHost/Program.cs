using Projects;
using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);
var api = builder.AddParameter("api", secret: true);

var postgres = builder.AddPostgres("postgres", username, password)
  .WithDataVolume(isReadOnly: false);

var postgresdb = postgres.AddDatabase("postgresdb");

builder.AddProject<Projects.Players_API>("apiservice")
       .WithReference(postgresdb)
       .WaitFor(postgresdb)
       .WithEnvironment("API_KEY", api);

builder.AddProject<Projects.Playes_MigrationService>("migrations")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

builder.Build().Run();
