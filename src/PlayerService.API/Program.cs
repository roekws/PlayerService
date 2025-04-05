using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PlayerService.API.Infrastructure.Context;
using PlayerService.API.Infrastructure.Validation;
using PlayerService.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// var certPath = Environment.GetEnvironmentVariable("CERT_PATH");

// if (string.IsNullOrEmpty(certPath))
// {
//   Console.WriteLine("Certificate path not found");
//   return;
// }

// builder.WebHost.ConfigureKestrel(options =>
// {
//   options.ListenAnyIP(80); // HTTP
//   options.ListenAnyIP(443, listenOptions => // HTTPS
//   {
//     listenOptions.UseHttps(certPath, Environment.GetEnvironmentVariable("CERT_PASS"));
//   });
// });

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

builder.Services.AddHealthChecks();

builder.Services.AddScoped<PlayerRequestContext>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var connection = $"Host={Environment.GetEnvironmentVariable("DB_HOST")};" +
  $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
  $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
  $"Username={Environment.GetEnvironmentVariable("DB_USER")};" +
  $"Password={Environment.GetEnvironmentVariable("DB_PASSWORD")};";

builder.Services.AddDbContext<PlayerContext>(options => options.UseNpgsql(connection));

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

app.MapHealthChecks("/health");

app.MapControllers();

app.Run();

public partial class Program { }
