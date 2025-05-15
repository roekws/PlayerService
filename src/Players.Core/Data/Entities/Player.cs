using Players.Core.Common;

namespace Players.Core.Entities;

public class Player : BaseEntity
{
  public long DotaId { get; set; }

  public long SteamId { get; set; }

  public string PublicName { get; set; } = "Anonym";

  public bool IsPublicForLadder { get; set; } = false;

  public int CharactersLimit { get; set; } = 5;

  public ICollection<Character> Characters { get; set; } = []; // Collection navigation containing dependents
}
