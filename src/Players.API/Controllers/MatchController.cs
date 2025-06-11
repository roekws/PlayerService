using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Infrastructure.Errors;
using Players.API.Models;
using Players.Core.Services;

namespace Players.API.Controllers;

[ApiController]
[Route("api/match")]
public class MatchController(IMatchService matchService, IPlayerService playerService) : ControllerBase
{
  private readonly IMatchService matchService = matchService;
  private readonly IPlayerService playerService = playerService;

  [AllowAnonymous]
  [HttpGet("{id}")]
  [ProducesResponseType<MatchDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> GetMatchById(long id, bool detailed)
  {
    var match = await matchService.GetByIdAsync(id, detailed);

    if (match == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.MatchNotFound));
    }

    return Ok(new MatchDto(match));
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpPost()]
  [ProducesResponseType<MatchDto>(StatusCodes.Status201Created)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult> CreateMatch()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);
    var gameClientVersion = long.Parse(User.FindFirst(PlayersClaimTypes.GameClientVersion)!.Value);

    var player = await playerService.GetByDotaSteamIdsAsync(dotaId, steamId);

    if (player == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    var activeMatch = await matchService.GetActiveByPlayerId(player.Id, detailed: false);

    if (activeMatch != null)
    {
      return NotFound(new ErrorResponse(ApiErrors.ActiveMatchExists));
    }

    var addMatch = await matchService.CreateMatch(player.Id, gameClientVersion);

    if (addMatch == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.MatchCreationError));
    }

    return Ok(new MatchDto(addMatch));
  }
}
