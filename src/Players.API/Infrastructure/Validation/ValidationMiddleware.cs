using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Players.API.Infrastructure.Validation;

public class ValidationMiddleware(RequestDelegate next, IHostEnvironment env)
{
  private readonly RequestDelegate _next = next;
  private readonly IHostEnvironment _env = env;

  public async Task InvokeAsync(HttpContext context)
  {
    // Skip validation for Scalar/OpenAPI in and Development
    bool isDocumentationRoute = context.Request.Path.StartsWithSegments("/scalar") ||
      context.Request.Path.StartsWithSegments("/openapi");

    if (_env.IsDevelopment() && isDocumentationRoute)
    {
      await _next(context);
      return;
    }

    if (!IsValidGameClient(context) && !IsValidPublicClient(context))
    {
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      return;
    }

    await _next(context);
  }

  public static class AuthHeaders
  {
    public const string DedicatedKey = "X-Dedicated-Server-Key";
    public const string PublicKey = "X-Api-Key";
    public const string DotaId = "X-Dota-Id";
  }

  private bool IsValidGameClient(HttpContext context)
  {
    if (!context.Request.Headers.TryGetValue(AuthHeaders.DedicatedKey, out var keyHeader))
    {
      return false;
    }

    if (!ValidateKey(keyHeader.ToString(), "SERVER_KEY"))
    {
      return false;
    }

    if (!context.Request.Headers.TryGetValue(AuthHeaders.DotaId, out var dotaIdHeader))
    {
      return false;
    }

    if (!long.TryParse(dotaIdHeader, out var dotaId))
    {
      return false;
    }

    context.Items["Client"] = "Game";
    context.Items["DotaId"] = dotaId;
    return true;
  }

  private bool IsValidPublicClient(HttpContext context)
  {
    if (!context.Request.Headers.TryGetValue(AuthHeaders.PublicKey, out var keyHeader))
    {
      return false;
    }

    if (!ValidateKey(keyHeader.ToString(), "PUBLIC_KEY"))
    {
      return false;
    }

    context.Items["Client"] = "Public";
    return true;
  }

  private bool ValidateKey(string providedKey, string environmentKeyName)
  {
    var validKey = Environment.GetEnvironmentVariable(environmentKeyName);

    if (string.IsNullOrEmpty(providedKey) || string.IsNullOrEmpty(validKey))
    {
      return false;
    }

    var providedKeyBytes = Encoding.UTF8.GetBytes(providedKey);
    var validKeyBytes = Encoding.UTF8.GetBytes(validKey);

    return CryptographicOperations.FixedTimeEquals(providedKeyBytes, validKeyBytes);
  }
}
