namespace Players.Core.Entities;

public class City
{
  public long Id { get; set; }

  public int Level { get; set; }
  public int Health { get; set; }

  // Navigation properties
  public Match Match { get; set; } = null!;
  public ICollection<Building> Buildings { get; set; } = [];
}
