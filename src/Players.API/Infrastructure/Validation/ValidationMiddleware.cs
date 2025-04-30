using Players.API.Infrastructure.Context;
using Players.API.Infrastructure.Errors;

namespace Players.API.Infrastructure.Validation;

public class ValidationMiddleware(RequestDelegate next, IHostEnvironment env)
{
  private readonly RequestDelegate _next = next;
  private readonly IHostEnvironment _env = env;

  public async Task InvokeAsync(HttpContext context)
  {
    // Skip validation for Scalar/OpenAPI routes
    // Skip in Development only
    if (_env.IsDevelopment() &&
        context.Request.Path.StartsWithSegments("/scalar") ||
        context.Request.Path.StartsWithSegments("/openapi"))
    {
      await _next(context);
      return;
    }

    // Validate Dedicated key
    var key = context.Request.Headers["X-Dedicated-Server-Key"].ToString();
    if (string.IsNullOrEmpty(key) || !IsValidDedicatedKey(key))
    {
      context.Response.StatusCode = StatusCodes.Status403Forbidden;
      return;
    }

    //Validate Dota Id
    if (!context.Request.Headers.TryGetValue("X-Dota-Id", out var dotaIdHeader))
    {
      context.Response.StatusCode = StatusCodes.Status403Forbidden;
      return;
    }

    if (!long.TryParse(dotaIdHeader, out var dotaId))
    {
      context.Response.StatusCode = 400;
      await context.Response.WriteAsJsonAsync(ApiErrorResponse.Create(ApiErrors.InvalidDotaId));
      return;
    }

    var playerContext = context.RequestServices.GetRequiredService<PlayerRequestContext>();
    playerContext.DotaId = dotaId;

    await _next(context);
  }

  private bool IsValidDedicatedKey(string inputKey)
  {
    var validKey = Environment.GetEnvironmentVariable("API_KEY");
    return string.Equals(inputKey, validKey, StringComparison.Ordinal);
  }
}
