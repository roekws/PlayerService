using System.Text.Json.Serialization;
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
  public string? EnemyCitySnapshotJson { get; set; }

  public string? PlayerCharacterSnapshotJson { get; set; }
  public string? PlayerCitySnapshotJson { get; set; }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public BattleState State { get; set; }

  // Navigation properties
  [JsonIgnore]
  public Match Match { get; set; } = null!;
  [JsonIgnore]
  public Player? EnemyPlayer { get; set; }
}
