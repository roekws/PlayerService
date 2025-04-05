using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.API.Infrastructure.Context;
using PlayerService.API.Infrastructure.Errors;
using PlayerService.Core.Data;
using PlayerService.Core.Entities;

namespace PlayerService.API.Controllers;

[ApiController]
[Route("api/player")]
public class PlayerController(PlayerContext context, PlayerRequestContext player) : ControllerBase
{
  private readonly PlayerContext _context = context;
  private readonly PlayerRequestContext _player = player;


  [HttpGet("exists")]
  public async Task<Results<Ok, NotFound<ApiErrorResponse>>> IsPlayerExist()
  {
    var player = await _context.Players.AnyAsync(player => player.DotaId == _player.DotaId);

    if (!player)
    {
      return TypedResults.NotFound(ApiErrorResponse.Create(ApiErrors.PlayerNotFound));
    }

    return TypedResults.Ok();
  }

  [HttpPost]
  public async Task<Results<Created, BadRequest<ApiErrorResponse>>> AddPlayer()
  {
    var player = await _context.Players.AnyAsync(player => player.DotaId == _player.DotaId);

    if (player)
    {
      return TypedResults.BadRequest(ApiErrorResponse.Create(ApiErrors.PlayerExists));
    }

    var AddPlayer = new Player() { DotaId = _player.DotaId };
    _context.Players.Add(AddPlayer);
    await _context.SaveChangesAsync();

    return TypedResults.Created();
  }
}
