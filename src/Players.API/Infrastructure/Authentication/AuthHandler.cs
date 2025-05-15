using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Infrastructure.Errors;
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
      return AuthenticateResult.Fail(ApiErrors.KeyIsInvalid);
    }

    if (!Request.Headers.TryGetValue(AuthHeaders.DotaId, out var inputDotaId))
    {
      return AuthenticateResult.Fail(ApiErrors.DotaIsMissing);
    }

    if (!long.TryParse(inputDotaId.ToString(), out var dotaId))
    {
      return AuthenticateResult.Fail(ApiErrors.DotaIdIsInvalid);
    }

    if (!Request.Headers.TryGetValue(AuthHeaders.SteamId, out var inputSteamId))
    {
      return AuthenticateResult.Fail(ApiErrors.SteamIdIsMissing);
    }

    if (!long.TryParse(inputSteamId.ToString(), out var steamId))
    {
      return AuthenticateResult.Fail(ApiErrors.SteamIdIsInvalid);
    }

    claims.Add(new Claim(PlayersClaimTypes.DotaId, dotaId.ToString()));
    claims.Add(new Claim(PlayersClaimTypes.SteamId, steamId.ToString()));

    var identity = new ClaimsIdentity(claims, "GameAuth");
    var principal = new ClaimsPrincipal(identity);
    return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
  }
}
