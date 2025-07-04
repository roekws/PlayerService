namespace Players.Core.Data.Errors;

public static class MatchErrors
{
  public static Error NotFound =>
    new(ErrorType.NotFound, "#match_not_found", "The requested match could not be found");

  public static Error ActiveMatchExists =>
    new(ErrorType.Validation, "#active_match_exists", "Player already has an active match in progress");

  public static Error NotActive =>
    new(ErrorType.Validation, "#match_not_active", "Match is not active");

  public static Error VersionOutdated =>
    new(ErrorType.Validation, "#match_version_outdated", "Last active match version is outdated, match will be terminated");

  public static Error RetrieveFailed =>
    new(ErrorType.Failure, "#match_retrieve_failed", "Failed to fetch match data");

  public static Error CreateFailed =>
    new(ErrorType.Failure, "#cant_create_new_match", "Failed to create a new match");

  public static Error UpdateFailed =>
    new(ErrorType.Failure, "#match_update_failed", "Failed to update match data");

  public static Error DeleteFailed =>
    new(ErrorType.Failure, "#match_delete_failed", "Failed to delete match");
}
