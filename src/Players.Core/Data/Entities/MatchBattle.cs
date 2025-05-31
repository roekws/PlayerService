using Players.Core.Enums;

namespace Players.Core.Entities;

public class MatchBattle
{
  public long Id { get; set; }

  public long MatchId { get; set; }

  public long? EnemyPlayerId { get; set; }

  public DateTime StartTime { get; set; } = DateTime.UtcNow;
  public DateTime? EndTime { get; set; }

  public int CharacterHealthChange { get; set; } = 0;
  public int BaseHealthChange { get; set; } = 0;
  public int GoldChange { get; set; } = 0;
  public int ExperienceChange { get; set; } = 0;
  public int Number { get; set; } = 1;

  public string? EnemyCharacterSnapshotJson { get; set; }
  public string? EnemyBaseSnapshotJson { get; set; }

  public string? PlayerCharacterSnapshotJson { get; set; }
  public string? PlayerBaseSnapshotJson { get; set; }

  public BattleState State { get; set; }

  // Navigation properties
  public Match Match { get; set; } = null!;
  public Player? EnemyPlayer { get; set; }
}
