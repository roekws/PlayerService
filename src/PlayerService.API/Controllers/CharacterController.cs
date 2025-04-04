using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Data;
using PlayerService.Core.Entities;
using PlayerService.Core.Enums;

namespace PlayerService.Controllers;

// Middleware guarantees HttpContext.Items["DotaId"] is a non-null long.
[ApiController]
[Route("api/character")]
public class CharacterController(PlayerContext context) : ControllerBase
{
  private readonly PlayerContext _context = context;

  [HttpGet]
  public async Task<Results<Ok<List<Character>>, NotFound>> GetCharacters()
  {
    var characters = await _context.Characters
      .Where(character => character.PlayerId == (long)HttpContext.Items["DotaId"]!)
      .OrderBy(character => character.CreatedAt)
      .ToListAsync();

    return TypedResults.Ok(characters);
  }

  [HttpPost]
  public async Task<Results<Created, BadRequest>> AddCharacter(Hero hero)
  {
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    if (player == null)
    {
      return TypedResults.BadRequest();
    }

    var charactersAmount = await _context.Characters
      .Where(character => character.PlayerId == dotaId)
      .OrderBy(character => character.CreatedAt)
      .CountAsync();

    if (charactersAmount >= player.CharactersLimit)
    {
      return TypedResults.BadRequest();
    }

    var addCharacter = new Character() { PlayerId = dotaId, Hero = hero };
    _context.Characters.Add(addCharacter);
    await _context.SaveChangesAsync();

    return TypedResults.Created(addCharacter.Id.ToString());
  }
}
