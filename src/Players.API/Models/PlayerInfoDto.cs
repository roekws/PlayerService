namespace Players.API.Models;

public record PlayerInfoDto(
  long Id,
  bool IsPublicForLadder,
  string? DotaId = "Anonym",
  string? SteamId = "Anonym",
  string? PublicName = "Anonym",
  string? CreatedAt = "Anonym"
);
