using Players.Core.Entities;

namespace Players.API.Models;

public class PlayerDto
{
  public long Id { get; set; }

  public string? DotaId { get; set; }
  public string? SteamId { get; set; }

  public string? PublicName { get; set; }
  public bool IsPublicForLadder { get; set; }

  public double Rating { get; set; }

  public DateTime? LastActivityTime { get; set; }
  public DateTime? CreatedAt { get; set; }

  public string? CurrentMatchId { get; set; }

  public PlayerDto() { }

  public PlayerDto(Player player, bool anonymous = false)
  {
    var showPrivateData = !anonymous || player.IsPublicForLadder;

    Id = player.Id;
    DotaId = showPrivateData ? player.DotaId.ToString() : "Anonym";
    SteamId = showPrivateData ? player.SteamId.ToString() : "Anonym";
    PublicName = player.PublicName;
    IsPublicForLadder = player.IsPublicForLadder;
    Rating = player.Rating;
    LastActivityTime = showPrivateData ? player.LastActivity : DateTime.MinValue;
    CreatedAt = showPrivateData ? player.CreatedAt : DateTime.MinValue;
    CurrentMatchId = showPrivateData ? player.CurrentMatchId.ToString() : "Anonym";
  }
}
