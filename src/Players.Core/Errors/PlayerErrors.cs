namespace Players.Core.Data.Results;

public static class PlayerErrors
{
  public static Error NotFound =>
    new("#player_not_found", "The requested player could not be found");

  public static Error AlreadyExists =>
    new("#player_already_exists", "A player with specified details already exists");

  public static Error NotPublic =>
    new("#player_profile_private", "The player's profile is not accessible");

  public static Error CreateFailed =>
    new("#player_create_failed", "Failed to create new player profile");

  public static Error UpdateFailed =>
    new("#player_update_failed", "Failed to update player data");

  public static Error DeleteFailed =>
    new("#player_delete_failed", "Failed to delete player profile");

  public static Error NoIdentifierProvided =>
    new("#no_player_identifier", "Must provide player ID, Steam ID, or Dota ID to update data");
}
