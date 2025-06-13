using Players.Core.Data.Errors;

namespace Players.Core.Data.Results;

public class Result
{
  public Error? Error { get; init; }
  public bool IsSuccess => Error == null;

  protected Result() { }

  protected Result(Error? error)
  {
    Error = error;
  }

  public static Result Success()
    => new();

  public static Result Failure(Error error)
    => new(error);

  public T Match<T>(
    Func<T> onSuccess,
    Func<Error, T> onFailure
  )
  {
    return IsSuccess
      ? onSuccess()
      : onFailure(Error!);
  }
}
