using System.Text.Json.Serialization;

namespace Players.Core.Entities;

public class BuildingAbility
{
  public long Id { get; set; }

  public long BuildingId { get; set; }

  public required string Name { get; set; }

  public long Level { get; set; } = 1;

  // Navigation properties
  [JsonIgnore]
  public Building Building { get; set; } = null!;
}
