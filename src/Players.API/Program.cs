using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Players.API.Infrastructure.Authentication;
using Players.API.Infrastructure.Authorization.Claims;
using Players.Core.Data;
using Players.Core.Services;
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
  options.AddPolicy(Policies.GameOnly, policy =>
    policy.RequireClaim(PlayersClaimTypes.ClientType, PlayersClientTypes.Game));

  options.AddPolicy(Policies.AdminOnly, policy =>
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

builder.Services.AddOpenApi(options =>
{
  options.AddOperationTransformer((operation, context, cancellationToken) =>
  {
    if (operation.Parameters == null)
    {
      operation.Parameters = new List<OpenApiParameter>();
    }

    var endpointMetadata = context.Description.ActionDescriptor.EndpointMetadata;

    // Skip if [AllowAnonymous] exists
    if (endpointMetadata.Any(em => em is AllowAnonymousAttribute))
    {
      return Task.CompletedTask;
    }

    operation.Parameters.Add(new OpenApiParameter
    {
      Name = AuthHeaders.DedicatedKey,
      In = ParameterLocation.Header,
      Required = true,
      Schema = new OpenApiSchema { Type = "string" }
    });

    operation.Parameters.Add(new OpenApiParameter
    {
      Name = AuthHeaders.DotaId,
      In = ParameterLocation.Header,
      Required = true,
      Schema = new OpenApiSchema { Type = "long" }
    });

    operation.Parameters.Add(new OpenApiParameter
    {
      Name = AuthHeaders.SteamId,
      In = ParameterLocation.Header,
      Required = true,
      Schema = new OpenApiSchema { Type = "long" }
    });

    operation.Parameters.Add(new OpenApiParameter
    {
      Name = AuthHeaders.GameClientVersion,
      In = ParameterLocation.Header,
      Required = true,
      Schema = new OpenApiSchema { Type = "long" }
    });

    return Task.CompletedTask;
  });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
  options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var connection = Environment.GetEnvironmentVariable("DB_CONNECTION");

builder.Services.AddDbContext<PlayerContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped<IPlayerService, PlayerService>();

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
