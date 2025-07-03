using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Models.Responses;
using Players.Core.Data.Results;
using Players.Core.Services;

namespace Players.API.Controllers;

[ApiController]
[Route("api/matches")]
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
  [HttpGet()]
  public async Task<IActionResult> GetPlayerActiveMatch()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);
    var globalPatchVersion = long.Parse(User.FindFirst(PlayersClaimTypes.GlobalPatchVersion)!.Value);

    var result = await matchService.GetActiveByPlayerAsync(dotaId, steamId, detailed: true, globalPatchVersion);

    return result.Match(
      onSuccess: match => Ok(new MatchDto(match)),
      onFailure: Problem
    );
  }

  [AllowAnonymous]
  [HttpGet("list")]
  [ProducesResponseType<PaginatedList<MatchDto>>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetMatchesPaginated(
    [FromQuery] long? dotaId,
    [FromQuery] long? steamId,
    [FromQuery] long? id,
    [FromQuery] bool detailed,
    [FromQuery] int page = 1,
    [FromQuery] int size = 20
  )
  {
    var result = await matchService.GetPaginatedByPlayerAsync(
      dotaId,
      steamId,
      id,
      detailed,
      page,
      size
    );

    return result.Match(
      onSuccess: paginatedList => Ok(paginatedList.Items.ConvertAll(match => new MatchDto(match))),
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
    var globalPatchVersion = long.Parse(User.FindFirst(PlayersClaimTypes.GlobalPatchVersion)!.Value);
    var balancePatchVersion = long.Parse(User.FindFirst(PlayersClaimTypes.BalancePatchVersion)!.Value);

    var result = await matchService.CreateMatchAsync(dotaId, steamId, globalPatchVersion, balancePatchVersion);

    return result.Match(
      onSuccess: match =>
        {
          var uri = new Uri($"{Request.Scheme}://{Request.Host}/api/match?id={match.Id}");
          return Created(uri, new MatchDto(match));
        },
      onFailure: Problem
     );
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpGet("all")]
  [ProducesResponseType<PaginatedList<MatchDto>>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetMatchesPaginated(
    [FromQuery] bool detailed,
    [FromQuery] int page = 1,
    [FromQuery] int size = 20
  )
  {
    var result = await matchService.GetPaginatedAllAsync(detailed, page, size);

    return result.Match(
      onSuccess: paginatedList => Ok(paginatedList.Items.ConvertAll(match => new MatchDto(match))),
      onFailure: Problem
    );
  }
}
