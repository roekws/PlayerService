using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

var connection = builder.Configuration.GetConnectionString("Database");

if (string.IsNullOrEmpty(connection))
{
  Console.WriteLine("Default connectnion not found");
  return;
}

builder.Services.AddDbContext<PlayerContext>(options => options.UseNpgsql(connection));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.MapControllers();

app.Run();
