namespace Players.Core.Data.Errors;

public static class CharacterErrors
{
  public static Error InvalidHero =>
    new(ErrorType.Validation, "#invalid_hero", "The selected hero is not valid or available");
}
