using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Players.API.Infrastructure.Context;
using Players.API.Infrastructure.Validation;
using Players.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowAll", policy =>
  {
    policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
  });
});

builder.Services.AddOpenApi();

builder.Services.AddScoped<PlayerRequestContext>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// var connection = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
//   $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
//   $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
//   $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
//   $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.AddNpgsqlDbContext<PlayerContext>(connectionName: "postgresdb");

var app = builder.Build();

// Apply migration on start
using (var scope = app.Services.CreateScope())
{
  var db = scope.ServiceProvider.GetRequiredService<PlayerContext>();
  await db.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
  app.UseCors("AllowAll");
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }
