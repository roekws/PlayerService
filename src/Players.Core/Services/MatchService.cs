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
      .Where(match => match.Id == id);

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    var match = await query.FirstOrDefaultAsync();

    return match == null ?
      Result<Match>.Failure(MatchErrors.NotFound) :
      Result<Match>.Success(match);
  }

  public async Task<Result<Match>> GetActiveByPlayerAsync(long dotaId, long steamId, bool detailed, long gameClientVersion)
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
      .Where(match =>
        match.PlayerId == player.Id &&
        match.State == MatchState.Active
      );

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    var match = await query.FirstOrDefaultAsync();

    if (match == null)
    {
      return Result<Match>.Failure(MatchErrors.NotFound);
    }

    if (match.GameClientVersion != gameClientVersion)
    {
      try
      {
        match.State = MatchState.SystemTerminate;

        await context.SaveChangesAsync();

        return Result<Match>.Failure(MatchErrors.VersionOutdated);
      }
      catch
      {
        return Result<Match>.Failure(MatchErrors.UpdateFailed);
      }
    }

    return Result<Match>.Success(match);
  }

  public async Task<Result<PaginatedList<Match>>> GetPaginatedByPlayerAsync(
    long? dotaId,
    long? steamId,
    long? id,
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  )
  {
    try
    {
      var playerQuery = _context.Players.AsQueryable();
      var matchQuery = _context.Matches.AsNoTracking();

      if (id.HasValue)
      {
        playerQuery = playerQuery.Where(player => player.Id == id.Value);
        matchQuery = matchQuery.Where(match => match.PlayerId == id.Value);
      }
      else if (steamId.HasValue)
      {
        playerQuery = playerQuery.Where(player => player.SteamId == steamId.Value);
        matchQuery = matchQuery.Where(match => match.Player.SteamId == steamId.Value);
      }
      else if (dotaId.HasValue)
      {
        playerQuery = playerQuery.Where(player => player.DotaId == dotaId.Value);
        matchQuery = matchQuery.Where(match => match.Player.DotaId == dotaId.Value);
      }
      else
      {
        return Result<PaginatedList<Match>>.Failure(PlayerErrors.NoIdentifierProvided);
      }

      if (!await playerQuery.AnyAsync())
      {
        return Result<PaginatedList<Match>>.Failure(PlayerErrors.NotFound);
      }

      if (detailed)
      {
        matchQuery = IncludeDetails(matchQuery);
      }

      var paginatedMatches = await PaginatedList<Match>.CreateAsync(matchQuery, pageIndex, pageSize);
      return Result<PaginatedList<Match>>.Success(paginatedMatches);
    }
    catch
    {
      return Result<PaginatedList<Match>>.Failure(MatchErrors.NotFound);
    }
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

    var existingActiveMatch = await _context.Matches
      .AsNoTracking()
      .AnyAsync(Match =>
        Match.PlayerId == player.Id &&
        Match.State == MatchState.Active
      );

    if (existingActiveMatch)
    {
      return Result<Match>.Failure(MatchErrors.ActiveMatchExists);
    }

    var match = new Match()
    {
      PlayerId = player.Id,
      GameClientVersion = gameClientVersion
    };

    try
    {
      _context.Matches.Add(match);
      await _context.SaveChangesAsync();
      return Result<Match>.Success(match);
    }
    catch
    {
      return Result<Match>.Failure(MatchErrors.CreateFailed);
    }
  }

  public async Task<Result<PaginatedList<Match>>> GetPaginatedAllAsync(
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  )
  {
    try
    {
      var matchQuery = _context.Matches.AsNoTracking();

      if (detailed)
      {
        matchQuery = IncludeDetails(matchQuery);
      }

      var paginatedMatches = await PaginatedList<Match>.CreateAsync(matchQuery, pageIndex, pageSize);
      return Result<PaginatedList<Match>>.Success(paginatedMatches);
    }
    catch
    {
      return Result<PaginatedList<Match>>.Failure(MatchErrors.NotFound);
    }
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

    try
    {
      match.Character = character;
      var rowsAffected = await _context.SaveChangesAsync();
      return Result<Character>.Success(character);
    }
    catch
    {
      return Result<Character>.Failure(CharacterErrors.CreateFailed);
    }
  }

  private static IQueryable<Match> IncludeDetails(IQueryable<Match> query)
  {
    return query
      .Include(match => match.Player)
      .Include(match => match.Character)
        .ThenInclude(character => character.Items)
      .Include(match => match.Character)
        .ThenInclude(character => character.Abilities)
      .Include(match => match.City)
        .ThenInclude(character => character.Buildings)
        .ThenInclude(city => city.Abilities)
      .AsSplitQuery();
  }
}
