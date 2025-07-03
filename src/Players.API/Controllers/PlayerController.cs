using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Models.Requests.Player;
using Players.API.Models.Responses;
using Players.Core.Data.Results;
using Players.Core.Services;

namespace Players.API.Controllers;

[ApiController]
[Route("api/players")]
public class PlayerController(IPlayerService playerService) : BaseController
{
  private readonly IPlayerService playerService = playerService;

  [AllowAnonymous]
  [HttpGet()]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetPlayer(
    [FromQuery] long? id,
    [FromQuery] long? dotaId,
    [FromQuery] long? steamId
  )
  {
    var result = await playerService.GetAsync(id, dotaId, steamId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpGet("all")]
  [ProducesResponseType<PaginatedList<PlayerDto>>(StatusCodes.Status200OK)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAllPlayersPaginated(
    [FromQuery] int page = 1,
    [FromQuery] int size = 20
  )
  {
    var result = await playerService.GetAllPaginatedListAsync(page, size);

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
      onSuccess: player =>
        {
          var uri = new Uri($"{Request.Scheme}://{Request.Host}/api/player?id={player.Id}");
          return Created(uri, new PlayerDto(player));
        },
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpGet("me")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> GetAuthenticatedPlayer()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var result = await playerService.GetAsync(null, dotaId, steamId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpPatch("edit")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
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
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
  public async Task<IActionResult> ChangePlayerId(
    [FromBody] ChangePlayerIdRequest request
  )
  {
    var result = await playerService.ChangeDotaSteamIdsAsync(request.Id, request.NewDotaId, request.NewSteamId);

    return result.Match(
      onSuccess: player => Ok(new PlayerDto(player)),
      onFailure: Problem
    );
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
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
  [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
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
