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
  public async Task<IResult> GetPlayerById(long id)
  {
    var player = await _context.Players.FindAsync(id);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    return Results.Ok(new PlayerInfoDto(player, anonymous: true));
  }

  [AllowAnonymous]
  [HttpGet("dota={dotaId}")]
  public async Task<IResult> GetPlayerByDotaId(long dotaId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    return Results.Ok(new PlayerInfoDto(player));
  }

  [AllowAnonymous]
  [HttpGet("steam={steamId}")]
  public async Task<IResult> GetPlayerBySteamId(long steamId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.SteamId == steamId);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    return Results.Ok(new PlayerInfoDto(player));
  }

  [Authorize(Policy = "GameOnly")]
  [HttpPost("register")]
  public async Task<IResult> RegisterPlayer()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await _context.Players.AnyAsync(player => player.DotaId == dotaId || player.SteamId == steamId);

    if (player)
    {
      return Results.BadRequest(new { Error = ApiErrors.PlayerExists });
    }

    var AddPlayer = new Player() { DotaId = dotaId, SteamId = steamId };
    _context.Players.Add(AddPlayer);
    await _context.SaveChangesAsync();

    return Results.Created();
  }

  [Authorize(Policy = "GameOnly")]
  [HttpGet]
  public async Task<IResult> GetPlayerSelf()
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await _context.Players.FirstOrDefaultAsync(player => player.SteamId == steamId);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    return Results.Ok(new PlayerInfoDto(player));
  }

  [Authorize(Policy = "GameOnly")]
  [HttpPatch("edit")]
  public async Task<IResult> EditPlayerPublicData(bool? isPublicForLadder, string? publicName)
  {
    var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
    var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

    var player = await _context.Players.FirstOrDefaultAsync(player => player.SteamId == steamId);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    player.PublicName = publicName ?? player.PublicName;
    player.IsPublicForLadder = isPublicForLadder ?? player.IsPublicForLadder;

    await _context.SaveChangesAsync();

    return Results.Ok();
  }

  [Authorize(Policy = "AdminOnly")]
  [HttpPatch("idchange")]
  public async Task<IResult> ChangePlayerId(long id, long newDotaId, long newSteamId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.Id == id);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    player.DotaId = newDotaId;
    player.SteamId = newSteamId;
    await _context.SaveChangesAsync();

    return Results.Ok();
  }

  [Authorize(Policy = "AdminOnly")]
  [HttpDelete("{id}")]
  public async Task<IResult> RemovePlayer(long id)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.Id == id);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    _context.Players.Remove(player);
    await _context.SaveChangesAsync();

    return Results.NoContent();
  }
}
