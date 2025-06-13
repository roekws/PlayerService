namespace Players.Core.Data.Results;

public static class CharacterErrors
{
  public static Error InvalidHero =>
    new("#invalid_hero", "The selected hero is not valid or available");
}
