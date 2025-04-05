using System.Text.Json.Serialization;
using PlayerService.Core.Common;
using PlayerService.Core.Enums;

namespace PlayerService.Core.Entities;

public class Character : BaseEntity
{
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public Hero Hero { get; set; }

  public int Level { get; set; } = 1;

  public int Experience { get; set; } = 0;

  public long PlayerId { get; set; }

  [JsonIgnore]
  public Player Player { get; set; } = null!; // Required reference navigation to principal
}

public record CharacterInfoDto(long Id, string Hero, int Level, int Experience);
