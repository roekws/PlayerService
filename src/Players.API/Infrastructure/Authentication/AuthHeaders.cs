namespace Players.API.Infrastructure.Authorization.Claims;

public static class AuthHeaders
{
  public const string DedicatedKey = "X-Dedicated-Server-Key";
  public const string DotaId = "X-Dota-Id";
  public const string SteamId = "X-Steam-Id";
  public const string GlobalPatchVersion = "X-Global-Patch-Version";
  public const string BalancePatchVersion = "X-Balance-Patch-Version";
}
