using Players.Core.Entities;

namespace Players.API.Models;

public class CityDto
{
  public long Id { get; set; }

  public int? Level { get; set; }
  public int? Health { get; set; }

  public List<BuildingDto> Buildings { get; set; } = [];

  public CityDto() { }

  public CityDto(City city)
  {
    Id = city.Id;
    Level = city.Level;
    Health = city.Health;
    Buildings = city.Buildings?.Select(a => new BuildingDto(a)).ToList() ?? [];
  }
}
