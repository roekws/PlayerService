using Players.Core.Entities;

namespace Players.API.Models;

public class BuildingAbilityDto
{
  public long Id { get; set; }

  public string? Name { get; set; }

  public long? Level { get; set; }

  public BuildingAbilityDto() { }

  public BuildingAbilityDto(BuildingAbility ability)
  {
    Id = ability.Id;
    Name = ability.Name;
    Level = ability.Level;
  }
}
