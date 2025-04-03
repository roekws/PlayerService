using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Data;
using PlayerService.Core.Entities;

namespace PlayerService.Controllers;

[ApiController]
[Route("api/player")]
public class PlayerController(PlayerContext context) : ControllerBase
{
  private readonly PlayerContext _context = context;

  [HttpGet]
  public async Task<Results<Ok<Player>, NotFound>> GetPlayer(long dotaId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    if (player == null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(player);
  }

  [HttpPost]
  public async Task<Results<Created, BadRequest>> AddPlayer(long dotaId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    if (player != null)
    {
      return TypedResults.BadRequest();
    }

    var AddPlayer = new Player() { DotaId = dotaId };
    _context.Players.Add(AddPlayer);
    await _context.SaveChangesAsync();

    return TypedResults.Created(AddPlayer.Id.ToString());
  }
}
