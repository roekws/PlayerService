using Players.Core.Entities;

namespace Players.API.Models;

public class BuildingDto
{
  public long Id { get; set; }

  public string? Name { get; set; }

  public int? Level { get; set; }
  public int? Experience { get; set; }
  public int? Health { get; set; }

  public int? GridX { get; set; }
  public int? GridY { get; set; }
  public int? GridZ { get; set; }
  public int? Rotation { get; set; }
  public List<BuildingAbilityDto> Abilities { get; set; } = [];

  public BuildingDto() { }

  public BuildingDto(Building building)
  {
    Id = building.Id;
    Name = building.Name;
    Level = building.Level;
    Experience = building.Experience;
    Health = building.Health;
    GridX = building.GridX;
    GridY = building.GridY;
    GridZ = building.GridZ;
    Rotation = building.Rotation;
    Abilities = building.Abilities?.Select(a => new BuildingAbilityDto(a)).ToList() ?? [];
  }
}
