namespace Players.Core.Data.Errors;

public static class PlayerErrors
{
  public static Error NotFound =>
    new(ErrorType.NotFound, "#player_not_found", "The requested player could not be found");

  public static Error AlreadyExists =>
    new(ErrorType.Validation, "#player_already_exists", "A player with specified details already exists");

  public static Error NotPublic =>
    new(ErrorType.AccessForbidden, "#player_profile_private", "The player's profile is not accessible");

  public static Error PublicNameInvalid =>
    new(ErrorType.Validation, "#player_public_name_invalid", "Public name must be 1-20 english letters only");

  public static Error RetrieveFailed =>
    new(ErrorType.Failure, "#player_retrieve_failed", "Failed to fetch player data");

  public static Error CreateFailed =>
    new(ErrorType.Failure, "#player_create_failed", "Failed to create new player profile");

  public static Error UpdateFailed =>
    new(ErrorType.Failure, "#player_update_failed", "Failed to update player data");

  public static Error DeleteFailed =>
    new(ErrorType.Failure, "#player_delete_failed", "Failed to delete player profile");

  public static Error NoIdentifierProvided =>
    new(ErrorType.Validation, "#player_no_identifier", "Must provide player ID, Steam ID, or Dota ID to update data");

  public static Error NoChangesProvided =>
    new(ErrorType.Validation, "#player_no_changes", "No changes provided for update");
}
