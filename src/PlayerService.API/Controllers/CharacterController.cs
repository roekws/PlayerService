using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerService.API.Infrastructure.Context;
using PlayerService.API.Infrastructure.Errors;
using PlayerService.Core.Data;
using PlayerService.Core.Entities;
using PlayerService.Core.Enums;

namespace PlayerService.API.Controllers;

[ApiController]
[Route("api/character")]
public class CharacterController(PlayerContext context, PlayerRequestContext player) : ControllerBase
{
  private readonly PlayerContext _context = context;
  private readonly PlayerRequestContext _player = player;


  [HttpGet("list")]
  public async Task<Results<Ok<List<CharacterInfoDto>>, NotFound<ApiErrorResponse>>> GetCharacters()
  {
    var player = await _context.Players.AnyAsync(player => player.DotaId == _player.DotaId);

    if (!player)
    {
      return TypedResults.NotFound(ApiErrorResponse.Create(ApiErrors.PlayerNotFound));
    }

    var characters = await _context.Characters
      .Where(character => character.PlayerId == _player.DotaId)
      .OrderBy(character => character.CreatedAt)
      .Select(c => new CharacterInfoDto(c.Id, c.Hero.ToString(), c.Level, c.Experience))
      .ToListAsync();

    return TypedResults.Ok(characters);
  }

  [HttpGet("{characterId}")]
  public async Task<Results<Ok, NotFound<ApiErrorResponse>>> GetCharacter(long characterId)
  {
    var character = await _context.Characters
      .FirstOrDefaultAsync(c => c.Id == characterId && c.PlayerId == _player.DotaId);

    if (character == null)
    {
      return TypedResults.NotFound(ApiErrorResponse.Create(ApiErrors.CharacterNotFound));
    }

    return TypedResults.Ok();
  }

  [HttpPost("{characterId}/exp-change")]
  public async Task<Results<Ok, NotFound<ApiErrorResponse>>> ChangeExp(long characterId, int expChange)
  {
    var character = await _context.Characters
      .FirstOrDefaultAsync(c => c.Id == characterId && c.PlayerId == _player.DotaId);

    if (character == null)
    {
      return TypedResults.NotFound(ApiErrorResponse.Create(ApiErrors.CharacterNotFound));
    }

    character.Experience += expChange;
    await _context.SaveChangesAsync();

    return TypedResults.Ok();
  }

  [HttpPost("{characterId}/level-up")]
  public async Task<Results<Ok<Character>, NotFound<ApiErrorResponse>>> LevelUp(long characterId)
  {
    var character = await _context.Characters
      .FirstOrDefaultAsync(c => c.Id == characterId && c.PlayerId == _player.DotaId);

    if (character == null)
    {
      return TypedResults.NotFound(ApiErrorResponse.Create(ApiErrors.CharacterNotFound));
    }

    character.Level += 1;
    await _context.SaveChangesAsync();

    return TypedResults.Ok(character);
  }

  [HttpPost("create")]
  public async Task<Results<Created, NotFound<ApiErrorResponse>, BadRequest<ApiErrorResponse>>> AddCharacter(string createHero)
  {
    var playerInfo = await _context.Players
        .Where(p => p.DotaId == _player.DotaId)
        .Select(p => new
        {
          Exists = true,
          CanCreate = p.Characters.Count < p.CharactersLimit,
        })
        .FirstOrDefaultAsync();

    if (playerInfo == null)
    {
      return TypedResults.NotFound(ApiErrorResponse.Create(ApiErrors.PlayerNotFound));
    }

    if (!playerInfo.CanCreate)
    {
      return TypedResults.BadRequest(ApiErrorResponse.Create(ApiErrors.CharacterLimitReached));
    }

    if (!Enum.TryParse(createHero, out Hero hero))
    {
      return TypedResults.BadRequest(ApiErrorResponse.Create(ApiErrors.InvalidHero));
    }

    var addCharacter = new Character() { PlayerId = _player.DotaId, Hero = hero };
    _context.Characters.Add(addCharacter);
    await _context.SaveChangesAsync();

    return TypedResults.Created();
  }

  [HttpDelete("delete")]
  public async Task<Results<NoContent, BadRequest<ApiErrorResponse>, NotFound<ApiErrorResponse>>> DeleteCharacter(long characterId)
  {
    var player = await _context.Players.FirstOrDefaultAsync(player => player.DotaId == _player.DotaId);

    if (player == null)
    {
      return TypedResults.BadRequest(ApiErrorResponse.Create(ApiErrors.PlayerNotFound));
    }

    var deleteCharacter = await _context.Characters.FirstOrDefaultAsync(character => character.Id == characterId);

    if (deleteCharacter == null)
    {
      return TypedResults.NotFound(ApiErrorResponse.Create(ApiErrors.CharacterNotFound));
    }

    if (deleteCharacter.PlayerId != player.Id)
    {
      return TypedResults.BadRequest(ApiErrorResponse.Create(ApiErrors.CharacterNotFound));
    }

    _context.Characters.Remove(deleteCharacter);
    await _context.SaveChangesAsync();

    return TypedResults.NoContent();
  }
}
