using System.Text.Json.Serialization;
using Players.Core.Enums;

namespace Players.Core.Entities;

public class Character
{
  public long Id { get; set; }

  [JsonConverter(typeof(JsonStringEnumConverter))]
  public Hero Hero { get; set; }

  public int Level { get; set; } = 1;
  public int Experience { get; set; } = 0;
  public int Health { get; set; } = 100;
  public int Gold { get; set; } = 5;

  // Navigation properties
  [JsonIgnore]
  public Match Match { get; set; } = null!;
  public ICollection<CharacterItem> Items { get; set; } = [];
  public ICollection<CharacterAbility> Abilities { get; set; } = [];
}
