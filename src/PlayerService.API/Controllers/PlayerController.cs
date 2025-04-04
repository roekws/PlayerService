using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Data;
using PlayerService.Core.Entities;

namespace PlayerService.Controllers;

// Middleware guarantees HttpContext.Items["DotaId"] is a non-null long.
[ApiController]
[Route("api/player")]
public class PlayerController(PlayerContext context) : ControllerBase
{
  private readonly PlayerContext _context = context;

  [HttpGet("login")]
  public async Task<Results<Ok, NotFound>> IsPlayerExist()
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.AnyAsync(player => player.DotaId == dotaId);

    // Player not found
    if (!player)
    {
      return TypedResults.NotFound();
    }

    // Player exist
    return TypedResults.Ok();
  }

  [HttpPost("register")]
  public async Task<Results<Created, BadRequest>> AddPlayer()
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.AnyAsync(player => player.DotaId == dotaId);

    // Player already exist
    if (player)
    {
      return TypedResults.BadRequest();
    }

    var AddPlayer = new Player() { DotaId = dotaId };
    _context.Players.Add(AddPlayer);
    await _context.SaveChangesAsync();

    // Player created
    return TypedResults.Created();
  }
}
