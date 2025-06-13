using Players.Core.Data.Errors;

namespace Players.Core.Data.Results;

public class Result<T>
{
  public T? Value { get; init; }
  public Error? Error { get; init; }
  public bool IsSuccess => Error == null;

  public TResult Match<TResult>(
    Func<T, TResult> onSuccess,
    Func<Error, TResult> onFailure
  )
  {
    return IsSuccess
      ? onSuccess(Value!)
      : onFailure(Error!);
  }
}
