using Microsoft.EntityFrameworkCore;
using Players.Core.Data;
using Players.Core.Entities;
using Players.Core.Data.Results;
using Players.Core.Data.Errors;
using Players.Core.Enums;

namespace Players.Core.Services;

public class MatchService(PlayerContext context) : IMatchService
{
  private readonly PlayerContext _context = context;

  public async Task<Result<Match>> GetByIdAsync(long id, bool detailed)
  {
    var query = _context.Matches
      .AsNoTracking()
      .Where(m => m.Id == id);

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    var match = await query.FirstOrDefaultAsync();

    return match == null ?
      Result<Match>.Failure(MatchErrors.NotFound) :
      Result<Match>.Success(match);
  }

  public async Task<Result<Match>> GetActiveByPlayerAsync(long dotaId, long steamId, bool detailed)
  {
    var player = await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player =>
        player.DotaId == dotaId &&
        player.SteamId == steamId
      );

    if (player == null)
    {
      return Result<Match>.Failure(PlayerErrors.NotFound);
    }

    var query = _context.Matches
      .AsNoTracking()
      .Where(m => m.PlayerId == player.Id);

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    var match = await query.FirstOrDefaultAsync();

    return match == null ?
      Result<Match>.Failure(MatchErrors.NotFound) :
      Result<Match>.Success(match);
  }

  public async Task<PaginatedList<Match>> GetPaginatedByPlayerAsync(
    long dotaId,
    long steamId,
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  )
  {
    var player = await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player =>
        player.DotaId == dotaId &&
        player.SteamId == steamId
      );

    var query = _context.Matches
      .AsNoTracking()
      .Where(m =>
        m.Player.DotaId == dotaId &&
        m.Player.SteamId == steamId
        );

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    return await PaginatedList<Match>.CreateAsync(query, pageIndex, pageSize);
  }

  public async Task<Result<Match>> CreateMatchAsync(long dotaId, long steamId, long gameClientVersion)
  {
    var player = await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player =>
        player.DotaId == dotaId &&
        player.SteamId == steamId
      );

    if (player == null)
    {
      return Result<Match>.Failure(PlayerErrors.NotFound);
    }

    var match = new Match()
    {
      PlayerId = player.Id,
      GameClientVersion = gameClientVersion
    };

    _context.Matches.Add(match);

    var rowsAffected = await _context.SaveChangesAsync();

    return rowsAffected > 0 ?
      Result<Match>.Success(match) :
      Result<Match>.Failure(MatchErrors.CreateFailed);
  }

  public async Task<Result<Character>> CreateCharacterAsync(long id, Hero hero)
  {
    var match = await _context.Matches.FindAsync(id);

    if (match == null)
    {
      return Result<Character>.Failure(MatchErrors.NotFound);
    }

    if (match.State != MatchState.Active)
    {
      return Result<Character>.Failure(MatchErrors.NotActive);
    }

    if (match.CharacterId != null)
    {
      return Result<Character>.Failure(CharacterErrors.AlreadyExisted);
    }

    if (match.CharacterId != null)
    {
      return Result<Character>.Failure(CharacterErrors.AlreadyExisted);
    }

    var character = new Character
    {
      Hero = hero
    };

    match.Character = character;

    var rowsAffected = await _context.SaveChangesAsync();

    return rowsAffected > 0 ?
      Result<Character>.Success(character) :
      Result<Character>.Failure(CharacterErrors.CreateFailed);
  }

  private static IQueryable<Match> IncludeDetails(IQueryable<Match> query)
  {
    return query
      .Include(m => m.Player)
      .Include(m => m.Character)
        .ThenInclude(c => c.Items)
      .Include(m => m.Character)
        .ThenInclude(c => c.Abilities)
      .Include(m => m.City)
        .ThenInclude(c => c.Buildings)
        .ThenInclude(b => b.Abilities)
      .AsSplitQuery();
  }
}
