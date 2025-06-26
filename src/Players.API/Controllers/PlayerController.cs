using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Models.Requests.Player;
using Players.API.Models.Responses;
using Players.Core.Data.Results;
using Players.Core.Services;

namespace Players.API.Controllers;

[ApiController]
[Route("api/player")]
public class PlayerController(IPlayerService playerService) : BaseController
{
  private readonly IPlayerService playerService = playerService;

  [AllowAnonymous]
  [HttpGet("{id}")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetPlayerById(long id)
  {
    var result = await playerService.GetByIdAsync(id);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [AllowAnonymous]
  [HttpGet("dota={dotaId}")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetPlayerByDotaId(long dotaId)
  {
    var result = await playerService.GetByDotaIdAsync(dotaId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [AllowAnonymous]
  [HttpGet("steam={steamId}")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetPlayerBySteamId(long steamId)
  {
    var result = await playerService.GetBySteamIdAsync(steamId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpGet("all")]
  [ProducesResponseType<PaginatedList<PlayerDto>>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAllPlayersPaginated(
    [FromQuery] int page = 1,
    [FromQuery] int size = 20
  )
  {
    var result = await playerService.GetAllPaginatedList(page, size);

    return result.Match(
      onSuccess: paginatedList => Ok(paginatedList.Items.ConvertAll(player => new PlayerDto(player))),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpPost("register")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status201Created)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> RegisterPlayer()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var result = await playerService.RegisterAsync(dotaId, steamId);

    return result.Match(
      onSuccess: player => CreatedAtAction(
          actionName: nameof(GetPlayerById),
          routeValues: new { id = player.Id },
          value: new PlayerDto(player)
        ),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpGet]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAuthenticatedPlayer()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var result = await playerService.GetByDotaSteamIdsAsync(dotaId, steamId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  // TO DO: Name rules
  [Authorize(Policy = Policies.GameOnly)]
  [HttpPatch("edit")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> UpdatePlayerPublicData(
    [FromBody] UpdatePlayerDataRequest request
  )
  {
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var result = await playerService.UpdatePublicDataAsync(request.IsPublicForLadder, request.PublicName, steamId: steamId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpPatch("idchange")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> ChangePlayerId(
    [FromBody] ChangePlayerIdRequest request
  )
  {
    var result = await playerService.ChangeDotaSteamIds(request.Id, request.NewDotaId, request.NewSteamId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> RemovePlayer(long id)
  {
    var result = await playerService.DeleteByIdAsync(id);

    return result.Match(
      onSuccess: NoContent,
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpDelete()]
  [ProducesResponseType<BatchDeleteResult>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> RemovePlayers(
    [FromBody] long[] ids
  )
  {
    var result = await playerService.BatchDeleteAsync(ids);

    return result.Match(
      onSuccess: Ok,
      onFailure: Problem
    );
  }
}
