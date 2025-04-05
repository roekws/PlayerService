namespace PlayerService.API.Infrastructure.Errors;

public class ApiErrorResponse
{
  public required string Error { get; set; }
  public string? Message { get; set; }

  public static ApiErrorResponse Create(string error, string? message = null)
  {
    return new ApiErrorResponse { Error = error, Message = message };
  }
}
