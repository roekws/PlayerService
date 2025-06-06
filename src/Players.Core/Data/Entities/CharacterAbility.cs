namespace Players.Core.Entities;

public class CharacterAbility
{
  public long Id { get; set; }

  public long CharacterId { get; set; }

  public required string Name { get; set; }

  public long Level { get; set; } = 1;

  // Navigation properties
  public Character Character { get; set; } = null!;
}
