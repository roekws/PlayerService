using Microsoft.EntityFrameworkCore;
using Players.Core.Data.Pagination;
using Players.Core.Entities;
using Players.Core.Services;

namespace Players.Core.Data;

public class PlayerService(PlayerContext context) : IPlayerService
{
  private readonly PlayerContext _context = context;

  public async Task<Player?> GetByIdAsync(long id)
  {
    return await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player => player.Id == id);
  }

  public async Task<Player?> GetByDotaIdAsync(long dotaId)
  {
    return await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player => player.DotaId == dotaId);
  }

  public async Task<Player?> GetBySteamIdAsync(long steamId)
  {
    return await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player => player.SteamId == steamId);
  }
  public async Task<Player?> GetByDotaSteamIdsAsync(long dotaId, long steamId)
  {
    return await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player =>
        player.DotaId == dotaId &&
        player.SteamId == steamId
      );
  }

  public async Task<PaginatedList<Player>> GetAllPaginatedList(
    int pageIndex = 1,
    int pageSize = 10
  )
  {
    return await PaginatedList<Player>.CreateAsync(
      _context.Players.AsNoTracking(),
      pageIndex,
      pageSize
    );
  }

  public async Task<Player?> RegisterAsync(long dotaId, long steamId)
  {
    var player = await _context.Players
      .AsNoTracking()
      .AnyAsync(
        player => player.DotaId == dotaId ||
        player.SteamId == steamId
      );

    if (player)
    {
      return null;
    }

    var AddPlayer = new Player() { DotaId = dotaId, SteamId = steamId };

    _context.Players.Add(AddPlayer);
    await _context.SaveChangesAsync();

    return AddPlayer;
  }

  public async Task<Player?> UpdatePublicDataAsync(bool? isPublicForLadder,
    string? publicName,
    long? id = null,
    long? steamId = null,
    long? dotaId = null
  )
  {
    var query = _context.Players.AsQueryable();

    if (id.HasValue)
    {
      query = query.Where(p => p.Id == id.Value);
    }
    else if (steamId.HasValue)
    {
      query = query.Where(p => p.SteamId == steamId.Value);
    }
    else if (dotaId.HasValue)
    {
      query = query.Where(p => p.DotaId == dotaId.Value);
    }
    else
    {
      return null;
    }

    var player = await query.FirstOrDefaultAsync();

    if (player == null)
    {
      return null;
    }

    player.PublicName = publicName ?? player.PublicName;
    player.IsPublicForLadder = isPublicForLadder ?? player.IsPublicForLadder;

    await _context.SaveChangesAsync();
    return player;
  }

  public async Task<Player?> ChangeDotaSteamIds(long id, long newDotaId, long newSteamId)
  {
    var player = await _context.Players.FindAsync(id);

    if (player == null)
    {
      return null;
    }

    player.DotaId = newDotaId;
    player.SteamId = newSteamId;
    await _context.SaveChangesAsync();

    return player;
  }

  public async Task<bool?> DeleteByIdAsync(long id)
  {
    var player = await _context.Players.FindAsync(id);

    if (player == null)
    {
      return null;
    }

    _context.Players.Remove(player);
    await _context.SaveChangesAsync();

    return true;
  }
}
