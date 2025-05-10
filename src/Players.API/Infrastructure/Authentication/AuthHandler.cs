using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Players.API.Infrastructure.Authorization.Claims;

namespace Players.API.Infrastructure.Authentication;

public class AuthHandler : AuthenticationHandler<AuthSchemeOptions>
{
  // Precomputed keys
  private static readonly byte[] _serverKeyBytes;
  private static readonly byte[] _adminKeyBytes;

  static AuthHandler()
  {
    _serverKeyBytes = GetKeyBytes("SERVER_KEY");
    _adminKeyBytes = GetKeyBytes("ADMIN_KEY");
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

    var providedKeyBytes = Encoding.UTF8.GetBytes(keyHeader.ToString());

    var claims = new List<Claim>();

    if (CryptographicOperations.FixedTimeEquals(providedKeyBytes, _serverKeyBytes))
    {
      claims.Add(new Claim(PlayersClaimTypes.ClientType, PlayersClientTypes.Game));
    }
    else if (CryptographicOperations.FixedTimeEquals(providedKeyBytes, _adminKeyBytes))
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

  private static byte[] GetKeyBytes(string envVarName)
  {
    var key = Environment.GetEnvironmentVariable(envVarName);

    if (!string.IsNullOrEmpty(key))
    {
      return Encoding.UTF8.GetBytes(key);
    }

    return Array.Empty<byte>();
  }
}
