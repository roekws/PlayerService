using Players.Core.Entities;

namespace Players.API.Models;

public class CharacterAbilityDto
{
  public long Id { get; set; }

  public string? Name { get; set; }

  public long? Level { get; set; }

  public CharacterAbilityDto() { }

  public CharacterAbilityDto(CharacterAbility ability)
  {
    Id = ability.Id;
    Name = ability.Name;
    Level = ability.Level;
  }
}
