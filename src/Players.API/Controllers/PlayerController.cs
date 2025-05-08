using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

  [HttpPost("{dotaId}")]
  public async Task<IResult> AddPlayer(long dotaId)
  {
    var player = await _context.Players.AnyAsync(player => player.DotaId == dotaId);

    if (player)
    {
      return Results.BadRequest(new { Error = ApiErrors.PlayerExists });
    }

    var AddPlayer = new Player() { DotaId = dotaId };
    _context.Players.Add(AddPlayer);
    await _context.SaveChangesAsync();

    return Results.Created();
  }

  [HttpGet("{dotaId}")]
  public async Task<IResult> GetPlayer(long dotaId)
  {
    var player = await _context.Players
      .Select(p => new PlayerInfoDto(p.PublicName, p.IsPublic, p.DotaId))
      .FirstOrDefaultAsync(player => player.DotaId == dotaId);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    if (player.IsPublic)
    {
      return Results.Ok(player);
    }

    return Results.Forbid();
  }

  [HttpPatch]
  public async Task<IResult> ChangePlpayerId(long dotaId, long newDotaId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    player.DotaId = newDotaId;
    await _context.SaveChangesAsync();

    return Results.NoContent();
  }

  [HttpDelete("{dotaId}")]
  public async Task<IResult> RemovePlayer(long dotaId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    if (player == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    _context.Players.Remove(player);
    await _context.SaveChangesAsync();

    return Results.NoContent();
  }

  [HttpGet("{dotaId}/exists")]
  public async Task<IResult> PlayerExist(long dotaId)
  {
    var player = await _context.Players.AnyAsync(player => player.DotaId == dotaId);

    if (!player)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    return Results.Ok();
  }
}
