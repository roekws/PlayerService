using PlayerService.API.Infrastructure.Context;
using PlayerService.API.Infrastructure.Errors;

namespace PlayerService.API.Infrastructure.Validation;

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
      // 404 Instead of 401 to Avoid information leakage
      context.Response.StatusCode = StatusCodes.Status404NotFound;
      await context.Response.WriteAsync("Not Found");
      return;
    }

    //Validate Dota Id
    if (!context.Request.Headers.TryGetValue("X-Dota-Id", out var dotaIdHeader))
    {
      context.Response.StatusCode = 404;
      await context.Response.WriteAsJsonAsync("Not Found");
      return;
    }

    if (!long.TryParse(dotaIdHeader, out var dotaId))
    {
      context.Response.StatusCode = 404;
      await context.Response.WriteAsJsonAsync(ApiErrorResponse.Create(ApiErrors.InvalidDotaId));
      return;
    }

    var playerContext = context.RequestServices.GetRequiredService<PlayerRequestContext>();
    playerContext.DotaId = dotaId;

    await _next(context);
  }

  private bool IsValidDedicatedKey(string key)
  {
    return true;
    // return key == Environment.GetEnvironmentVariable("API_KEY");
    // Can get key only after publishing to server
  }
}
