using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Players.API.Infrastructure.Validation;
using Players.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Validate configuraiton
var missingVariables = new List<string>();

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SERVER_KEY")))
  missingVariables.Add("SERVER_KEY");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("PUBLIC_KEY")))
  missingVariables.Add("PUBLIC_KEY");

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION")))
  missingVariables.Add("DB_CONNECTION");

if (missingVariables.Count > 0)
{
  throw new InvalidOperationException(
    $"Missing required environment variables: {string.Join(", ", missingVariables)}. ");
}

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

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var connection = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<PlayerContext>(options => options.UseNpgsql(connection));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseCors("AllowAll");
  app.MapOpenApi();
  app.MapScalarApiReference();

  // Apply migration on start
  using (var scope = app.Services.CreateScope())
  {
    var db = scope.ServiceProvider.GetRequiredService<PlayerContext>();
    await db.Database.MigrateAsync();
  }
}
else
{
  app.UseHttpsRedirection();
}

app.UseMiddleware<ValidationMiddleware>();

app.MapControllers();

app.Run();

