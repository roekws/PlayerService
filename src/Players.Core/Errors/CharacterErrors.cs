namespace Players.Core.Data.Errors;

public static class CharacterErrors
{
  public static Error InvalidHero =>
    new(ErrorType.Validation, "#invalid_hero", "The selected hero is not valid or available");

  public static Error AlreadyExisted =>
    new(ErrorType.Validation, "#already_exist", "Character already exists in specified match");

  public static Error CreateFailed =>
    new(ErrorType.Failure, "#cant_create_character", "Failed to create character");
}
