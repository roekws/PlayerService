namespace Players.Core.Data.Errors;

public static class MatchErrors
{
  public static Error NotFound =>
    new("#match_not_found", "The requested match could not be found");

  public static Error ActiveMatchExists =>
    new("#active_match_exists", "Player already has an active match in progress");

  public static Error CreationFailed =>
    new("#cant_create_new_match", "Failed to create a new match");

  public static Error UpdateFailed =>
    new("#match_update_failed", "Failed to update match data");

  public static Error DeleteFailed =>
    new("#match_delete_failed", "Failed to delete match");
}
