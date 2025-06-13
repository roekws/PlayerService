namespace Players.API.Models.Requests.Player;

public class ChangePlayerIdRequest
{
  public required long Id { get; set; }
  public required long NewDotaId { get; set; }
  public required long NewSteamId { get; set; }
}
