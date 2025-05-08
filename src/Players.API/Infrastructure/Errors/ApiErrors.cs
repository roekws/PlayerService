namespace Players.API.Infrastructure.Errors;

public static class ApiErrors
{
  // Player
  public const string PlayerExists = "#player_exists";
  public const string PlayerNotFound = "#player_not_found";
  public const string PlayerNotPublic = "#player_not_public";

  // Character
  public const string CharacterNotFound = "#character_not_found";
  public const string CharacterLimitReached = "#character_limit_reached";
  public const string InvalidHero = "#invalid_hero";

  // Validation
  public const string InvalidDotaId = "#invalid_dota_id";
}
