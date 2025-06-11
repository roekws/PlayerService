namespace Players.API.Infrastructure.Errors;

public static class ApiErrors
{
  // Player
  public const string PlayerExists = "#player_exists";
  public const string PlayerNotFound = "#player_not_found";
  public const string PlayerNotPublic = "#player_not_public";

  // match
  public const string MatchNotFound = "#match_not_found";
  public const string ActiveMatchExists = "#active_match_exists";
  public const string MatchCreationError = "#cant_create_new_match";

  // Character
  public const string CharacterNotFound = "#character_not_found";
  public const string CharacterLimitReached = "#character_limit_reached";
  public const string InvalidHero = "#invalid_hero";

  // Validation
  public const string DotaIsMissing = "#dota_id_is_missing";
  public const string DotaIdIsInvalid = "#invalid_dota_id";
  public const string SteamIdIsMissing = "#steam_id_is_missing";
  public const string SteamIdIsInvalid = "#invalid_steam_id";
  public const string KeyIsInvalid = "#invalid_key";
  public const string GameClientVersionIsMissing = "#game_client_version_is_missing";
  public const string GameClientVersionIsInvalid = "#invalid_game_client_version";
}
