using Microsoft.EntityFrameworkCore;
using Players.Core.Data;
using Players.Core.Data.Pagination;
using Players.Core.Entities;

namespace Players.Core.Services;

public class MatchService(PlayerContext context) : IMatchService
{
  private readonly PlayerContext _context = context;

  public async Task<Match?> GetByIdAsync(long id, bool detailed)
  {
    var query = _context.Matches
    .AsNoTracking()
    .Where(m => m.Id == id);

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    return await query.FirstOrDefaultAsync();
  }

  public async Task<Match?> GetActiveByPlayerId(long playerId, bool detailed)
  {
    var query = _context.Matches
      .AsNoTracking()
      .Where(m => m.PlayerId == playerId);

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    return await query.FirstOrDefaultAsync();
  }

  public async Task<PaginatedList<Match>> GetPaginatedByPlayerId(
    long playerId,
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  )
  {
    var query = _context.Matches
      .AsNoTracking()
      .Where(m => m.PlayerId == playerId);

    if (detailed)
    {
      query = IncludeDetails(query);
    }

    return await PaginatedList<Match>.CreateAsync(query, pageIndex, pageSize);
  }

  public async Task<Match?> CreateMatch(long playerId, long gameClientVersion)
  {
    var player = await _context.Players.FindAsync(playerId);

    if (player == null)
    {
      return null;
    }

    var match = new Match()
    {
      PlayerId = playerId,
      GameClientVersion = gameClientVersion
    };

    _context.Matches.Add(match);
    await _context.SaveChangesAsync();

    return match;
  }

  private static IQueryable<Match> IncludeDetails(IQueryable<Match> query)
  {
    return query
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
