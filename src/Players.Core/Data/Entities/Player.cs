namespace Players.Core.Entities;

public class Player
{
  public long Id { get; set; }

  public long DotaId { get; set; }
  public long SteamId { get; set; }

  public string PublicName { get; set; } = "Anonym";
  public bool IsPublicForLadder { get; set; } = false;

  public int Rating { get; set; } = 0;

  public DateTime LastActivity { get; set; } = DateTime.UtcNow;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public long CurrentMatchId { get; set; }

  // Navigation properties
  public ICollection<Match> Matches { get; set; } = [];
  public ICollection<MatchBattle> MatchesBattles { get; set; } = [];
}
