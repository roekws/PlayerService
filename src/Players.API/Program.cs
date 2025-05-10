using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Players.API.Infrastructure.Authentication;
using Players.API.Infrastructure.Authorization.Claims;
using Players.Core.Data;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Validate configuraiton
var missingVariables = new List<string>();

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SERVER_KEY")))
{
  missingVariables.Add("SERVER_KEY");
}

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DB_CONNECTION")))
{
  missingVariables.Add("DB_CONNECTION");
}

if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ADMIN_KEY")))
{
  missingVariables.Add("ADMIN_KEY");
}

if (missingVariables.Count > 0)
{
  throw new InvalidOperationException(
    $"Missing required environment variables: {string.Join(", ", missingVariables)}. ");
}

builder.Services
  .AddAuthentication("GameAuth")
  .AddScheme<AuthSchemeOptions, AuthHandler>("GameAuth", null);

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("GameOnly", policy =>
    policy.RequireClaim(PlayersClaimTypes.ClientType, PlayersClientTypes.Game));

  options.AddPolicy("AdminOnly", policy =>
    policy.RequireClaim(PlayersClaimTypes.ClientType, PlayersClientTypes.Admin));
});

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

app.UseAuthentication();
app.UseAuthorization();

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

app.MapControllers();

app.Run();
