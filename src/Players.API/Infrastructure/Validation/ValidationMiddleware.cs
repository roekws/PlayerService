using System.Security.Cryptography;
using System.Text;

namespace Players.API.Infrastructure.Validation;

public class ValidationMiddleware(RequestDelegate next, IHostEnvironment env)
{
  private readonly RequestDelegate _next = next;
  private readonly IHostEnvironment _env = env;

  public async Task InvokeAsync(HttpContext context)
  {
    // Skip validation for Scalar/OpenAPI in and Development
    bool isDocumentationRoute = context.Request.Path.StartsWithSegments("/scalar") || context.Request.Path.StartsWithSegments("/openapi");

    if (_env.IsDevelopment() && isDocumentationRoute)
    {
      await _next(context);
      return;
    }

    if (!IsValidDedicatedKey(context))
    {
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      return;
    }

    if (!IsValidDotaId(context))
    {
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      return;
    }

    await _next(context);
  }

  public static class AuthHeaders
  {
    public const string ApiKey = "X-Dedicated-Server-Key";
    public const string DotaId = "X-Dota-Id";
  }

  private bool IsValidDotaId(HttpContext context)
  {
    if (!context.Request.Headers.TryGetValue(AuthHeaders.DotaId, out var dotaIdHeader))
    {
      return false;
    }

    if (!long.TryParse(dotaIdHeader, out var dotaId))
    {
      return false;
    }

    return true;
  }

  private bool IsValidDedicatedKey(HttpContext context)
  {
    if (!context.Request.Headers.TryGetValue(AuthHeaders.ApiKey, out var keyHeader))
    {
      return false;
    }
    var keyHeaderString = keyHeader.ToString();

    var validKey = Environment.GetEnvironmentVariable("API_KEY");

    if (string.IsNullOrEmpty(keyHeaderString) || string.IsNullOrEmpty(validKey))
    {
      return false;
    }

    var keyHeaderBytes = Encoding.UTF8.GetBytes(keyHeaderString);
    var validKeyBytes = Encoding.UTF8.GetBytes(validKey);

    return CryptographicOperations.FixedTimeEquals(keyHeaderBytes, validKeyBytes);
  }
}
