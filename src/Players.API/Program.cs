using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Players.API.Infrastructure.Validation;
using Players.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

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

var connection = $"Host=players.database;" +
  $"Port=5432;" +
  $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
  $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
  $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

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

