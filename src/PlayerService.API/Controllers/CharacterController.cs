using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Data;
using PlayerService.Core.Entities;
using PlayerService.Core.Enums;

namespace PlayerService.Controllers;

[ApiController]
[Route("api/character")]
public class CharacterController(PlayerContext context) : ControllerBase
{
  private readonly PlayerContext _context = context;

  [HttpGet]
  public async Task<Results<BadRequest, Ok<List<Character>>, NotFound>> GetCharacters()
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    // Player not found
    if (player == null)
    {
      return TypedResults.BadRequest();
    }

    var characters = await _context.Characters
      .Where(character => character.PlayerId == dotaId)
      .OrderBy(character => character.CreatedAt)
      .ToListAsync();

    return TypedResults.Ok(characters);
  }

  [HttpPost]
  public async Task<Results<Created, BadRequest>> AddCharacter(Hero hero)
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    // Player not found
    if (player == null)
    {
      return TypedResults.BadRequest();
    }

    var charactersAmount = await _context.Characters
      .Where(character => character.PlayerId == dotaId)
      .OrderBy(character => character.CreatedAt)
      .CountAsync();

    // Players not allowed to create more characters than his limit
    if (charactersAmount >= player.CharactersLimit)
    {
      return TypedResults.BadRequest();
    }

    var addCharacter = new Character() { PlayerId = dotaId, Hero = hero };
    _context.Characters.Add(addCharacter);
    await _context.SaveChangesAsync();

    return TypedResults.Created(addCharacter.Id.ToString());
  }

  [HttpDelete]
  public async Task<Results<Created, BadRequest>> DeleteCharacter(long heroId)
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    // Player not found
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
