using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Infrastructure.Errors;
using Players.API.Models;
using Players.Core.Data.Pagination;
using Players.Core.Services;

namespace Players.API.Controllers;

[ApiController]
[Route("api/player")]
public class PlayerController(IPlayerService playerService) : ControllerBase
{
  private readonly IPlayerService playerService = playerService;

  [AllowAnonymous]
  [HttpGet("{id}")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> GetPlayerById(long id)
  {
    var player = await playerService.GetByIdAsync(id);

    if (player == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    return Ok(new PlayerDto(player, anonymous: true));
  }

  [AllowAnonymous]
  [HttpGet("dota={dotaId}")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> GetPlayerByDotaId(long dotaId)
  {
    var player = await playerService.GetByDotaIdAsync(dotaId);

    if (player == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    return Ok(new PlayerDto(player));
  }

  [AllowAnonymous]
  [HttpGet("steam={steamId}")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  public async Task<ActionResult<PlayerDto>> GetPlayerBySteamId(long steamId)
  {
    var player = await playerService.GetBySteamIdAsync(steamId);

    if (player == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    return Ok(new PlayerDto(player));
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpGet("all")]
  [ProducesResponseType<PaginatedList<PlayerDto>>(StatusCodes.Status200OK)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> GetAllPlayersPaginated(
    [FromQuery] int page = 1,
    [FromQuery] int size = 20
  )
  {
    var players = await playerService.GetAllPaginatedList(page, size);
    players.Items.ConvertAll(player => new PlayerDto(player));

    return Ok(players);
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpPost("register")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status201Created)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> RegisterPlayer()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await playerService.RegisterAsync(dotaId, steamId);

    if (player == null)
    {
      return BadRequest(new ErrorResponse(ApiErrors.PlayerExists));
    }

    return CreatedAtAction(
      actionName: nameof(GetPlayerById),
      routeValues: new { id = player.Id },
      value: new PlayerDto(player)
    );
  }

  [Authorize(Policy = Policies.GameOnly)]
  [HttpGet]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> GetAuthenticatedPlayer()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await playerService.GetByDotaSteamIdsAsync(dotaId, steamId);

    if (player == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    return Ok(new PlayerDto(player));
  }

  // TO DO: Name rules
  [Authorize(Policy = Policies.GameOnly)]
  [HttpPatch("edit")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> UpdatePlayerPublicData(bool? isPublicForLadder, string? publicName)
  {
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await playerService.UpdatePublicDataAsync(isPublicForLadder, publicName, steamId: steamId);

    if (player == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    return Ok(new PlayerDto(player));
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpPatch("idchange")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> ChangePlayerId(long id, long newDotaId, long newSteamId)
  {
    var player = await playerService.ChangeDotaSteamIds(id, newDotaId, newSteamId);

    if (player == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    return Ok(new PlayerDto(player));
  }

  [Authorize(Policy = Policies.AdminOnly)]
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> RemovePlayer(long id)
  {
    var deleteResult = await playerService.DeleteByIdAsync(id);

    if (deleteResult == null)
    {
      return NotFound(new ErrorResponse(ApiErrors.PlayerNotFound));
    }

    return NoContent();
  }
}
