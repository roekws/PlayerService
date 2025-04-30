namespace Players.API.Infrastructure.Errors;

public class ApiErrorResponse
{
  public required string Error { get; set; }

  public static ApiErrorResponse Create(string error)
  {
    return new ApiErrorResponse { Error = error };
  }
}
