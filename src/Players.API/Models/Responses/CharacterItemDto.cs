using Players.Core.Entities;

namespace Players.API.Models.Responses;

public class CharacterItemDto
{
  public long Id { get; set; }

  public string? Name { get; set; }

  public CharacterItemDto() { }

  public CharacterItemDto(CharacterItem item)
  {
    Id = item.Id;
    Name = item.Name;
  }
}
