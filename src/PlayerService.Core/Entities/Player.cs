using System.Diagnostics.Contracts;

namespace PlayerService.Core.Entities;

public class Player
{
  public long Id { get; set; }

  public long DotaId { get; set; }

  public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.Now;
}
