using PlayerService.Core.Common;
using PlayerService.Core.Enums;

namespace PlayerService.Core.Entities;

public class Character : BaseEntity
{
  public Hero Hero { get; set; }

  public int Level { get; set; } = 1;

  public int Expirience { get; set; } = 0;

  public long PlayerId { get; set; }

  public Player Player { get; set; } = null!; // Required reference navigation to principal
}
