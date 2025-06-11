using Players.Core.Entities;

namespace Players.API.Models;

public class CharacterDto
{
  public long Id { get; set; }
  public string? Hero { get; set; }
  public int? Level { get; set; }
  public int? Experience { get; set; }
  public int? Health { get; set; }
  public int? Gold { get; set; }
  public List<CharacterItemDto> Items { get; set; } = [];
  public List<CharacterAbilityDto> Abilities { get; set; } = [];

  public CharacterDto() { }

  public CharacterDto(Character character)
  {
    Id = character.Id;
    Hero = character.Hero.ToString();
    Level = character.Level;
    Experience = character.Experience;
    Health = character.Health;
    Gold = character.Gold;
    Items = character.Items?.Select(i => new CharacterItemDto(i)).ToList() ?? [];
    Abilities = character.Abilities?.Select(a => new CharacterAbilityDto(a)).ToList() ?? [];
  }
}
