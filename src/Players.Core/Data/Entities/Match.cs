using Players.Core.Enums;

namespace Players.Core.Entities;

public class Match
{
  public long Id { get; set; }

  public long PlayerId { get; set; }
  public long CharacterId { get; set; }
  public long BaseId { get; set; }

  public DateTime StartTime { get; set; } = DateTime.UtcNow;
  public DateTime? EndTime { get; set; }

  public int Level { get; set; } = 0;
  public int RatingChange { get; set; } = 0;

  public int GameClientVersion { get; set; }

  public MatchResultType Result { get; set; }

  // Navigation properties
  public Player Player { get; set; } = null!;
  public Character Character { get; set; } = null!;
  public Base Base { get; set; } = null!;
  public ICollection<MatchBattle> Battles { get; set; } = [];
}
