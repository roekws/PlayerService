namespace Players.Core.Data.Errors;

public static class MatchErrors
{
  public static Error NotFound =>
    new(ErrorType.NotFound, "#match_not_found", "The requested match could not be found");

  public static Error ActiveMatchExists =>
    new(ErrorType.Validation, "#active_match_exists", "Player already has an active match in progress");

  public static Error CreationFailed =>
    new(ErrorType.Failure, "#cant_create_new_match", "Failed to create a new match");

  public static Error UpdateFailed =>
    new(ErrorType.Failure, "#match_update_failed", "Failed to update match data");

  public static Error DeleteFailed =>
    new(ErrorType.Failure, "#match_delete_failed", "Failed to delete match");
}
