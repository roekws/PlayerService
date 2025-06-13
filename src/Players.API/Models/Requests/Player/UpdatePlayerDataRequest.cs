namespace Players.API.Models.Requests.Player;

public class UpdatePlayerDataRequest
{
  public bool? IsPublicForLadder { get; set; }
  public string? PublicName { get; set; }
}
