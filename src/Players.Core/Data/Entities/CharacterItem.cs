namespace Players.Core.Entities;

public class CharacterItem
{
  public long Id { get; set; }

  public long CharacterId { get; set; }

  public required string Name { get; set; }

  // Navigation properties
  public Character Character { get; set; } = null!;
}
