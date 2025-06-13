using Players.Core.Data.Errors;

namespace Players.Core.Data.Results;

public class Result<T> : Result
{
  public T? Value { get; init; }

  public Result(T? value)
    : base()
  {
    Value = value;
  }

  public Result(Error? error) : base(error)
  {
    Value = default;
  }

  public static Result<T> Success(T value)
    => new(value);

  public static new Result<T> Failure(Error error)
    => new(error);

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
