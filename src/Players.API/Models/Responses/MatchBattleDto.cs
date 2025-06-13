using Players.Core.Entities;

namespace Players.API.Models.Responses;

public class MatchBattleDto
{
  public long Id { get; set; }

  public long? MatchId { get; set; }

  public long? EnemyPlayerId { get; set; }

  public DateTime? StartTime { get; set; }
  public DateTime? EndTime { get; set; }

  public int? CharacterHealthChange { get; set; }
  public int? BaseHealthChange { get; set; }
  public int? GoldChange { get; set; }
  public int? ExperienceChange { get; set; }
  public int? Number { get; set; }

  public string? EnemyCharacterSnapshotJson { get; set; }
  public string? EnemyCitySnapshotJson { get; set; }

  public string? PlayerCharacterSnapshotJson { get; set; }
  public string? PlayerCitySnapshotJson { get; set; }

  public string? State { get; set; }

  public MatchBattleDto() { }

  public MatchBattleDto(MatchBattle battle)
  {
    Id = battle.Id;
    MatchId = battle.MatchId;
    EnemyPlayerId = battle.EnemyPlayerId;
    StartTime = battle.StartTime;
    EndTime = battle.EndTime;
    CharacterHealthChange = battle.CharacterHealthChange;
    BaseHealthChange = battle.BaseHealthChange;
    GoldChange = battle.GoldChange;
    ExperienceChange = battle.ExperienceChange;
    Number = battle.Number;
    EnemyCharacterSnapshotJson = battle.EnemyCharacterSnapshotJson;
    EnemyCitySnapshotJson = battle.EnemyCitySnapshotJson;
    PlayerCharacterSnapshotJson = battle.PlayerCharacterSnapshotJson;
    PlayerCitySnapshotJson = battle.PlayerCitySnapshotJson;
    State = battle.State.ToString();
  }
}
