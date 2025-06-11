using Players.Core.Entities;

namespace Players.API.Models;

public class MatchDto
{
  public long Id { get; set; }

  public long? PlayerId { get; set; }
  public long? CharacterId { get; set; }
  public long? CityId { get; set; }

  public DateTime? StartTime { get; set; }
  public DateTime? EndTime { get; set; }

  public int? Level { get; set; }
  public int? RatingChange { get; set; }

  public long? GameClientVersion { get; set; }

  public string? State { get; set; }

  public PlayerDto? Player { get; set; }
  public CharacterDto? Character { get; set; }
  public CityDto? City { get; set; }
  public List<MatchBattleDto> Battles { get; set; } = [];

  public MatchDto() { }

  public MatchDto(Match match)
  {
    Id = match.Id;
    PlayerId = match.PlayerId;
    CharacterId = match.CharacterId;
    CityId = match.CityId;
    StartTime = match.StartTime;
    EndTime = match.EndTime;
    Level = match.Level;
    RatingChange = match.RatingChange;
    GameClientVersion = match.GameClientVersion;
    State = match.State.ToString();

    Player = match.Player != null ? new PlayerDto(match.Player) : null;
    Character = match.Character != null ? new CharacterDto(match.Character) : null;
  }
}
