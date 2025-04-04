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

  [HttpGet("list")]
  public async Task<Results<Ok<List<CharacterInfoDto>>, BadRequest, NotFound>> GetCharacters()
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.AnyAsync(player => player.DotaId == dotaId);

    // Player not found
    if (!player)
    {
      return TypedResults.BadRequest();
    }

    var characters = await _context.Characters
      .Where(character => character.PlayerId == dotaId)
      .OrderBy(character => character.CreatedAt)
      .Select(c => new CharacterInfoDto(c.Id, c.Hero, c.Level, c.Expirience))
      .ToListAsync();

    // Characters list found
    return TypedResults.Ok(characters);
  }

  [HttpGet("{characterId}")]
  public async Task<Results<Ok<Character>, NotFound>> GetCharacter(long characterId)
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var character = await _context.Characters
      .FirstOrDefaultAsync(c => c.Id == characterId && c.PlayerId == dotaId);

    if (character == null)
    {
      return TypedResults.NotFound();
    }

    return TypedResults.Ok(character);
  }

  [HttpPost("create")]
  public async Task<Results<Created, BadRequest>> AddCharacter(string createHero)
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var hasAvailableSlots = await _context.Players
        .Where(p => p.DotaId == dotaId)
        .Select(p => p.Characters.Count < p.CharactersLimit)
        .FirstOrDefaultAsync();

    // Player not found or character amount hit limit
    if (!hasAvailableSlots || !Enum.TryParse(createHero, out Hero hero))
    {
      return TypedResults.BadRequest();
    }

    var addCharacter = new Character() { PlayerId = dotaId, Hero = hero };
    _context.Characters.Add(addCharacter);
    await _context.SaveChangesAsync();

    // Character created
    return TypedResults.Created();
  }

  [HttpDelete("delete")]
  public async Task<Results<NoContent, BadRequest, NotFound>> DeleteCharacter(long characterId)
  {
    // Middleware guarantees HttpContext.Items["DotaId"] is a non-null long
    var dotaId = (long)HttpContext.Items["DotaId"]!;

    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == dotaId);

    // Player not found
    if (player == null)
    {
      return TypedResults.BadRequest();
    }

    var deleteCharacter = await _context.Characters.FirstOrDefaultAsync(character => character.Id == characterId);

    // Character not found
    if (deleteCharacter == null)
    {
      return TypedResults.NotFound();
    }

    // Player not allowed to delete character of another player
    if (deleteCharacter.PlayerId != player.Id)
    {
      return TypedResults.BadRequest();
    }

    _context.Characters.Remove(deleteCharacter);
    await _context.SaveChangesAsync();

    // Character deleted
    return TypedResults.NoContent();
  }
}
