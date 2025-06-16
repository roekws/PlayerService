using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Models.Responses;
using Players.Core.Services;

namespace Players.API.Controllers;

[ApiController]
[Route("api/match")]
public class MatchController(IMatchService matchService, IPlayerService playerService) : BaseController
{
  private readonly IMatchService matchService = matchService;
  private readonly IPlayerService playerService = playerService;

  [AllowAnonymous]
  [HttpGet("{id}")]
  [ProducesResponseType<MatchDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetMatchById(long id, bool detailed)
  {
    var result = await matchService.GetByIdAsync(id, detailed);

    return result.Match(
      onSuccess: match => Ok(new MatchDto(match)),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpPost()]
  [ProducesResponseType<MatchDto>(StatusCodes.Status201Created)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> CreateMatch()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);
    var gameClientVersion = long.Parse(User.FindFirst(PlayersClaimTypes.GameClientVersion)!.Value);

    var result = await matchService.CreateMatchAsync(dotaId, steamId, gameClientVersion);

    return result.Match(
       onSuccess: match => Ok(new MatchDto(match)),
       onFailure: Problem
     );
  }
}
