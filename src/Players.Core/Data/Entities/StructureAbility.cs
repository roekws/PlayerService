namespace Players.Core.Entities;

public class StructureAbility
{
  public long Id { get; set; }

  public long StructureId { get; set; }

  public required string Name { get; set; }

  public long Level { get; set; } = 1;

  // Navigation properties
  public Structure Structure { get; set; } = null!;
}
