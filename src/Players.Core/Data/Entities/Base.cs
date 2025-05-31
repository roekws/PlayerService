namespace Players.Core.Entities;

public class Base
{
  public long Id { get; set; }

  public int Level { get; set; }
  public int Health { get; set; }

  // Navigation properties
  public Match Match { get; set; } = null!;
  public ICollection<Structure> Structures { get; set; } = [];
}
