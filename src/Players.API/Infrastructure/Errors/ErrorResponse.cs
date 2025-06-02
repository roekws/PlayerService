namespace Players.API.Infrastructure.Errors;

public class ErrorResponse
{
  public string ErrorCode { get; set; }

  public ErrorResponse(string errorCode)
  {
    ErrorCode = errorCode;
  }
}
