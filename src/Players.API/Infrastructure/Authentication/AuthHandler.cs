using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Players.API.Infrastructure.Authorization.Claims;
using Scalar.AspNetCore;

namespace Players.API.Infrastructure.Authentication;

public class AuthHandler : AuthenticationHandler<AuthSchemeOptions>
{
  // Precomputed keys
  private static readonly string _serverKey;
  private static readonly string _adminKey;

  static AuthHandler()
  {
    _serverKey = Environment.GetEnvironmentVariable("SERVER_KEY");
    _adminKey = Environment.GetEnvironmentVariable("ADMIN_KEY");
  }

  public AuthHandler(IOptionsMonitor<AuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
  {
  }

  protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
  {
    if (!Request.Headers.TryGetValue(AuthHeaders.DedicatedKey, out var keyHeader))
    {
      return AuthenticateResult.NoResult();
    }

    var claims = new List<Claim>();

    if (string.Equals(keyHeader.ToString(), _serverKey))
    {
      claims.Add(new Claim(PlayersClaimTypes.ClientType, PlayersClientTypes.Game));
    }
    else if (string.Equals(keyHeader.ToString(), _adminKey))
    {
      claims.Add(new Claim(PlayersClaimTypes.ClientType, PlayersClientTypes.Admin));
    }
    else
    {
      return AuthenticateResult.Fail("The provided authentication key is invalid");
    }

    if (!Request.Headers.TryGetValue(AuthHeaders.DotaId, out var inputDotaId))
    {
      return AuthenticateResult.Fail("Dota ID header is required but was not provided");
    }

    if (!long.TryParse(inputDotaId.ToString(), out var dotaId))
    {
      return AuthenticateResult.Fail("The provided Dota ID is not a valid number");
    }

    if (!Request.Headers.TryGetValue(AuthHeaders.SteamId, out var inputSteamId))
    {
      return AuthenticateResult.Fail(" Steam ID header is required but was not provided");
    }

    if (!long.TryParse(inputSteamId.ToString(), out var steamId))
    {
      return AuthenticateResult.Fail("The provided Steam ID is not a valid number");
    }

    if (!Request.Headers.TryGetValue(AuthHeaders.GlobalPatchVersion, out var inputGlobalPatchVersion))
    {
      return AuthenticateResult.Fail("Global patch version header is required");
    }

    if (!long.TryParse(inputGlobalPatchVersion.ToString(), out var globalPatchVersion))
    {
      return AuthenticateResult.Fail("Global patch version must be a valid number");
    }

    if (!Request.Headers.TryGetValue(AuthHeaders.BalancePatchVersion, out var inputBalancePatchtVersion))
    {
      return AuthenticateResult.Fail("Balance patch version header is required");
    }

    if (!long.TryParse(inputBalancePatchtVersion.ToString(), out var balancePatchVersion))
    {
      return AuthenticateResult.Fail("Balance patch version must be a valid number");
    }

    claims.Add(new Claim(PlayersClaimTypes.DotaId, dotaId.ToString()));
    claims.Add(new Claim(PlayersClaimTypes.SteamId, steamId.ToString()));
    claims.Add(new Claim(PlayersClaimTypes.GlobalPatchVersion, globalPatchVersion.ToString()));
    claims.Add(new Claim(PlayersClaimTypes.BalancePatchVersion, balancePatchVersion.ToString()));

    var identity = new ClaimsIdentity(claims, "GameAuth");
    var principal = new ClaimsPrincipal(identity);
    return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
  }
}
