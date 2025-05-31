// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Players.API.Infrastructure.Authorization.Claims;
// using Players.API.Infrastructure.Errors;
// using Players.API.Models;
// using Players.Core.Data;
// using Players.Core.Entities;
// using Players.Core.Enums;

// namespace Players.API.Controllers;

// [ApiController]
// [Route("api/character")]
// public class CharacterController(PlayerContext context) : ControllerBase
// {
//   private readonly PlayerContext _context = context;

//   [AllowAnonymous]
//   [HttpGet("{characterId}")]
//   public async Task<IResult> GetCharacter(long characterId)
//   {
//     var result = await _context.Characters
//       .Where(character => character.Id == characterId)
//       .Select(character => new
//       {
//         Character = new
//         {
//           character.Id,
//           character.Hero,
//           character.Level,
//           character.Experience,
//           character.CreatedAt
//         }
//       })
//       .FirstOrDefaultAsync();

//     if (result == null)
//     {
//       return Results.NotFound(new { Error = ApiErrors.CharacterNotFound });
//     }

//     return Results.Ok(new CharacterInfoDto(
//       result.Character.Id,
//       result.Character.Hero.ToString(),
//       result.Character.Level,
//       result.Character.Experience,
//       result.Character.CreatedAt.ToString()
//     ));
//   }

//   [AllowAnonymous]
//   [HttpGet("{playerId}/list")]
//   public async Task<IResult> GetCharacters(long playerId)
//   {
//     var charactersList = await _context.Characters
//       .Where(character => character.PlayerId == playerId)
//       .Select(character => new
//       {
//         character.Id,
//         character.Hero,
//         character.Level,
//         character.Experience,
//         character.CreatedAt
//       })
//       .OrderBy(character => character.CreatedAt)
//       .ToListAsync();

//     return Results.Ok(charactersList);
//   }

//   [Authorize(Policy = "GameOnly")]
//   [HttpPost("/create")]
//   public async Task<IResult> AddCharacter(string createHero)
//   {
//     var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
//     var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

//     var playerInfo = await _context.Players
//      .Where(player => player.SteamId == steamId)
//      .Select(player => new
//      {
//        player.Id,
//        player.CharactersLimit,
//      })
//      .FirstOrDefaultAsync();

//     if (playerInfo == null)
//     {
//       return Results.NotFound(new { Error = ApiErrors.PlayerNotFound });
//     }

//     var CharacterCount = await _context.Characters.CountAsync(c => c.PlayerId == playerInfo.Id);

//     if (CharacterCount >= playerInfo.CharactersLimit)
//     {
//       return Results.BadRequest(new { Error = ApiErrors.CharacterLimitReached });
//     }

//     if (!Enum.TryParse(createHero, out Hero hero))
//     {
//       return Results.BadRequest(new { Error = ApiErrors.InvalidHero });
//     }

//     var addCharacter = new Character() { PlayerId = playerInfo.Id, Hero = hero };
//     _context.Characters.Add(addCharacter);
//     await _context.SaveChangesAsync();

//     return Results.Created();
//   }

//   [Authorize(Policy = "GameOnly")]
//   [HttpPost("{characterId}/exp-change")]
//   public async Task<IResult> ChangeExp(long characterId, int? exp, int? level)
//   {
//     var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
//     var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

//     var character = await _context.Characters
//       .Where(c => c.Id == characterId && c.Player.SteamId == steamId)
//       .FirstOrDefaultAsync();

//     if (character == null)
//     {
//       return Results.NotFound(new { Error = ApiErrors.CharacterNotFound });
//     }

//     if (exp != null)
//     {
//       character.Experience += exp.Value;
//     }

//     if (level != null)
//     {
//       character.Level += level.Value;
//     }

//     await _context.SaveChangesAsync();

//     return Results.Ok();
//   }

//   [Authorize(Policy = "GameOnly")]
//   [HttpDelete("{characterId}/delete")]
//   public async Task<IResult> DeleteCharacter(long characterId)
//   {
//     var dotaId = long.Parse(User.FindFirst(PlayersClaimTypes.DotaId)!.Value);
//     var steamId = long.Parse(User.FindFirst(PlayersClaimTypes.SteamId)!.Value);

//     var character = await _context.Characters
//       .Where(c => c.Id == characterId && c.Player.SteamId == steamId)
//       .FirstOrDefaultAsync();

//     if (character == null)
//     {
//       return Results.NotFound(new { Error = ApiErrors.CharacterNotFound });
//     }

//     _context.Characters.Remove(character);
//     await _context.SaveChangesAsync();
//     return Results.NoContent();
//   }
// }
