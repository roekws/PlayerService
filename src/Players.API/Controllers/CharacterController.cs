using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Players.API.Infrastructure.Errors;
using Players.API.Models;
using Players.Core.Data;
using Players.Core.Entities;
using Players.Core.Enums;

namespace Players.API.Controllers;

[ApiController]
[Route("api/character")]
public class CharacterController(PlayerContext context) : ControllerBase
{
  private readonly PlayerContext _context = context;

  [HttpPost("{dotaId}/create")]
  public async Task<IResult> AddCharacter(long dotaId, string createHero)
  {
    var playerInfo = await _context.Players
        .Where(p => p.DotaId == dotaId)
        .Select(p => new
        {
          Exists = true,
          CanCreate = p.Characters.Count < p.CharactersLimit,
        })
        .FirstOrDefaultAsync();

    if (playerInfo == null)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    if (!playerInfo.CanCreate)
    {
      return Results.BadRequest(new { Error = ApiErrors.CharacterLimitReached });
    }

    if (!Enum.TryParse(createHero, out Hero hero))
    {
      return Results.BadRequest(new { Error = ApiErrors.InvalidDotaId });
    }

    var addCharacter = new Character() { PlayerId = dotaId, Hero = hero };
    _context.Characters.Add(addCharacter);
    await _context.SaveChangesAsync();

    return Results.Created();
  }

  [HttpGet("{characterId}")]
  public async Task<IResult> GetCharacter(long characterId)
  {
    var character = await _context.Characters
      .Where(c => c.Id == characterId)
      .Select(c => new CharacterInfoDto(c.Id, c.Hero.ToString(), c.Level, c.Experience))
      .FirstOrDefaultAsync();

    if (character == null)
    {
      return Results.NotFound(new { Error = ApiErrors.CharacterNotFound });
    }

    return Results.Ok(character);
  }

  [HttpGet("list/{dotaId}")]
  public async Task<IResult> GetCharacters(long dotaId)
  {
    var player = await _context.Players.AnyAsync(player => player.DotaId == dotaId);

    if (!player)
    {
      return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
    }

    var characters = await _context.Characters
      .Where(character => character.PlayerId == dotaId)
      .OrderBy(character => character.CreatedAt)
      .Select(c => new CharacterInfoDto(c.Id, c.Hero.ToString(), c.Level, c.Experience))
      .ToListAsync();

    return Results.Ok(characters);
  }

  [HttpPost("{characterId}/exp-change")]
  public async Task<IResult> ChangeExp(long characterId, int expChange)
  {
    var character = await _context.Characters
      .FirstOrDefaultAsync(c => c.Id == characterId);

    if (character == null)
    {
      return Results.NotFound(new { Error = ApiErrors.CharacterNotFound });
    }

    character.Experience += expChange;
    await _context.SaveChangesAsync();

    return Results.Ok();
  }

  [HttpPost("{characterId}/level-up")]
  public async Task<IResult> LevelUp(long characterId)
  {
    var character = await _context.Characters
      .FirstOrDefaultAsync(c => c.Id == characterId);

    if (character == null)
    {
      return Results.NotFound(new { Error = ApiErrors.CharacterNotFound });
    }

    character.Level += 1;
    await _context.SaveChangesAsync();

    return Results.Ok();
  }

  [HttpDelete("{characterId}/delete")]
  public async Task<IResult> DeleteCharacter(long characterId)
  {

    var deleteCharacter = await _context.Characters.FirstOrDefaultAsync(character => character.Id == characterId);

    if (deleteCharacter == null)
    {
      return Results.NotFound(new { Error = ApiErrors.CharacterNotFound });
    }

    _context.Characters.Remove(deleteCharacter);
    await _context.SaveChangesAsync();
    return Results.NoContent();
  }
}
