using Microsoft.EntityFrameworkCore;

namespace Players.Core.Data.Results;

public class Result<T>
{
  public T? Value { get; init; }
  public Error? Error { get; init; }
  public bool IsSuccess => Error == null;
}
