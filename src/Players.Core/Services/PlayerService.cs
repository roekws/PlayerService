using Microsoft.EntityFrameworkCore;
using Players.Core.Data.Errors;
using Players.Core.Data.Results;
using Players.Core.Entities;
using Players.Core.Services;

namespace Players.Core.Data;

public class PlayerService(PlayerContext context) : IPlayerService
{
  private readonly PlayerContext _context = context;

  public async Task<Result<Player>> GetByIdAsync(long id)
  {
    var player = await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player => player.Id == id);

    return player == null ?
      Result<Player>.Failure(PlayerErrors.NotFound) :
      Result<Player>.Success(player);
  }

  public async Task<Result<Player>> GetByDotaIdAsync(long dotaId)
  {
    var player = await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player => player.DotaId == dotaId);

    return player == null ?
      Result<Player>.Failure(PlayerErrors.NotFound) :
      Result<Player>.Success(player);
  }

  public async Task<Result<Player>> GetBySteamIdAsync(long steamId)
  {
    var player = await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player => player.SteamId == steamId);

    return player == null ?
      Result<Player>.Failure(PlayerErrors.NotFound) :
      Result<Player>.Success(player);
  }

  public async Task<Result<Player>> GetByDotaSteamIdsAsync(long dotaId, long steamId)
  {
    var player = await _context.Players
      .AsNoTracking()
      .FirstOrDefaultAsync(player =>
        player.DotaId == dotaId &&
        player.SteamId == steamId
      );

    return player == null ?
      Result<Player>.Failure(PlayerErrors.NotFound) :
      Result<Player>.Success(player);
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

  public async Task<Result<Player>> RegisterAsync(long dotaId, long steamId)
  {
    var player = await _context.Players
      .AsNoTracking()
      .AnyAsync(
        player => player.DotaId == dotaId ||
        player.SteamId == steamId
      );

    if (player)
    {
      return Result<Player>.Failure(PlayerErrors.NotFound);
    }

    var AddPlayer = new Player()
    {
      DotaId = dotaId,
      SteamId = steamId
    };

    _context.Players.Add(AddPlayer);
    var saveResult = await _context.SaveChangesAsync();

    return saveResult > 0 ?
      Result<Player>.Failure(PlayerErrors.CreateFailed) :
      Result<Player>.Success(AddPlayer);
  }

  public async Task<Result<Player>> UpdatePublicDataAsync(bool? isPublicForLadder,
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
      return Result<Player>.Failure(PlayerErrors.NoIdentifierProvided);
    }

    var player = await query.FirstOrDefaultAsync();

    if (player == null)
    {
      return Result<Player>.Failure(PlayerErrors.NotFound);
    }

    player.PublicName = publicName ?? player.PublicName;
    player.IsPublicForLadder = isPublicForLadder ?? player.IsPublicForLadder;

    var saveResult = await _context.SaveChangesAsync();

    return saveResult > 0 ?
      Result<Player>.Failure(PlayerErrors.UpdateFailed) :
      Result<Player>.Success(player);
  }

  public async Task<Result<Player>> ChangeDotaSteamIds(long id, long newDotaId, long newSteamId)
  {
    var player = await _context.Players.FindAsync(id);

    if (player == null)
    {
      return Result<Player>.Failure(PlayerErrors.NotFound);
    }

    player.DotaId = newDotaId;
    player.SteamId = newSteamId;

    var saveResult = await _context.SaveChangesAsync();

    return saveResult > 0 ?
      Result<Player>.Failure(PlayerErrors.CreateFailed) :
      Result<Player>.Success(player);
  }

  public async Task<Result> DeleteByIdAsync(long id)
  {
    var player = await _context.Players.FindAsync(id);

    if (player == null)
    {
      return Result.Failure(PlayerErrors.DeleteFailed);
    }

    _context.Players.Remove(player);

    var saveResult = await _context.SaveChangesAsync();

    return saveResult > 0 ?
      Result.Failure(PlayerErrors.DeleteFailed) :
      Result.Success();
  }
}
