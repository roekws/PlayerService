using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Players.API.Infrastructure.Authorization.Claims;

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
      return AuthenticateResult.Fail("Invalid key");
    }

    if (!Request.Headers.TryGetValue(AuthHeaders.DotaId, out var inputId))
    {
      return AuthenticateResult.Fail("Dota id not provided");
    }

    if (!long.TryParse(inputId.ToString(), out var dotaId))
    {
      return AuthenticateResult.Fail("Dota id is not valid");
    }

    claims.Add(new Claim(PlayersClaimTypes.DotaId, dotaId.ToString()));

    var identity = new ClaimsIdentity(claims, "GameAuth");
    var principal = new ClaimsPrincipal(identity);
    return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
  }
}
