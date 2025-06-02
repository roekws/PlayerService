using Players.Core.Entities;

namespace Players.API.Models;

public class PlayerInfoDto
{
  public long Id { get; set; }

  public string? DotaId { get; set; }
  public string? SteamId { get; set; }

  public string? PublicName { get; set; }
  public bool IsPublicForLadder { get; set; }

  public double Rating { get; set; }

  public string? LastActivityTime { get; set; }
  public string? CreatedAt { get; set; }

  public string? CurrentMatchId { get; set; }

  public PlayerInfoDto() { }

  public PlayerInfoDto(Player player, bool anonymous = false)
  {
    var showPrivateData = !anonymous || player.IsPublicForLadder;

    Id = player.Id;
    DotaId = showPrivateData ? player.DotaId.ToString() : "Anonym";
    SteamId = showPrivateData ? player.SteamId.ToString() : "Anonym";
    PublicName = player.PublicName;
    IsPublicForLadder = player.IsPublicForLadder;
    Rating = player.Rating;
    LastActivityTime = showPrivateData ? player.LastActivity.ToString("o") : "Anonym";
    CreatedAt = showPrivateData ? player.CreatedAt.ToString("o") : "Anonym";
    CurrentMatchId = showPrivateData ? player.CurrentMatchId.ToString() : "Anonym";
  }
}
