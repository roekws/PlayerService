namespace Players.Core.Entities;

public class Building
{
  public long Id { get; set; }

  public long CityId { get; set; }

  public required string Name { get; set; }

  public int Level { get; set; }
  public int Experience { get; set; }
  public int Health { get; set; }

  public int GridX { get; set; }
  public int GridY { get; set; }
  public int GridZ { get; set; }
  public int Rotation { get; set; }

  // Navigation properties
  public City City { get; set; } = null!;
  public ICollection<BuildingAbility> Abilities { get; set; } = [];
}
