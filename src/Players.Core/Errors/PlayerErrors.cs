namespace Players.Core.Data.Results;

public static class PlayerErrors
{
  public static Error NotFound =>
    new("#player_not_found", "The requested player could not be found");

  public static Error AlreadyExists =>
    new("#player_already_exists", "A player with specified details already exists");

  public static Error NotPublic =>
    new("#player_profile_private", "The player's profile is not accessible");
}
