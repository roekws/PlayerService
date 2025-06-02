using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Players.API.Infrastructure.Authorization.Claims;
using Players.API.Infrastructure.Errors;
using Players.API.Models;
using Players.Core.Data;
using Players.Core.Entities;

namespace Players.API.Controllers;

[ApiController]
[Route("api/player")]
public class PlayerController(PlayerContext context) : ControllerBase
{
  private readonly PlayerContext _context = context;

  [AllowAnonymous]
  [HttpGet("{id}")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  public async Task<ActionResult> GetPlayerById(long id)
  {
    var player = await _context.Players.FindAsync(id);

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
    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

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
    var player = await _context.Players.FirstOrDefaultAsync(player => player.SteamId == steamId);

    if (player == null)
    {
      return NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    return Ok(new PlayerDto(player));
  }

  [Authorize(Policy = "GameOnly")]
  [HttpPost("register")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status201Created)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> RegisterPlayer()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await _context.Players
      .AsNoTracking()
      .AnyAsync(
        player => player.DotaId == dotaId ||
        player.SteamId == steamId
      );

    if (player)
    {
      return BadRequest(new ErrorResponse(ApiErrors.PlayerExists));
    }

    var AddPlayer = new Player() { DotaId = dotaId, SteamId = steamId };
    _context.Players.Add(AddPlayer);
    await _context.SaveChangesAsync();

    return CreatedAtAction(
      actionName: nameof(GetPlayerById),
      routeValues: new { id = AddPlayer.Id },
      value: new PlayerDto(AddPlayer)
    );
  }

  [Authorize(Policy = "GameOnly")]
  [HttpGet]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> GetPlayerSelf()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await _context.Players.FirstOrDefaultAsync(player => player.SteamId == steamId);

    if (player == null)
    {
      return NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    return Ok(new PlayerDto(player));
  }

  // TO DO: Name rules
  [Authorize(Policy = "GameOnly")]
  [HttpPatch("edit")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> EditPlayerPublicData(bool? isPublicForLadder, string? publicName)
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await _context.Players.FirstOrDefaultAsync(player => player.SteamId == steamId);

    if (player == null)
    {
      return NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    player.PublicName = publicName ?? player.PublicName;
    player.IsPublicForLadder = isPublicForLadder ?? player.IsPublicForLadder;

    await _context.SaveChangesAsync();

    return Ok(new PlayerDto(player));
  }

  [Authorize(Policy = "AdminOnly")]
  [HttpPatch("idchange")]
  [ProducesResponseType<PlayerDto>(StatusCodes.Status200OK)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> ChangePlayerId(long id, long newDotaId, long newSteamId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.Id == id);

    if (player == null)
    {
      return NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    player.DotaId = newDotaId;
    player.SteamId = newSteamId;
    await _context.SaveChangesAsync();

    return Ok(new PlayerDto(player));
  }

  [Authorize(Policy = "AdminOnly")]
  [HttpDelete("{id}")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  public async Task<ActionResult<PlayerDto>> RemovePlayer(long id)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.Id == id);

    if (player == null)
    {
      return NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    _context.Players.Remove(player);
    await _context.SaveChangesAsync();

    return NoContent();
  }
}
