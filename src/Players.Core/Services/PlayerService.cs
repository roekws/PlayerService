using Microsoft.EntityFrameworkCore;
using Players.Core.Data.Errors;
using Players.Core.Data.Results;
using Players.Core.Entities;
using Players.Core.Services;

namespace Players.Core.Data;

public class PlayerService(PlayerContext context) : IPlayerService
{
  private readonly PlayerContext _context = context;

  public async Task<Result<Player>> GetAsync(long? id, long? dotaId, long? steamId)
  {
    var query = _context.Players.AsNoTracking();

    if (id.HasValue)
    {
      query = query.Where(player => player.Id == id.Value);
    }
    else if (steamId.HasValue)
    {
      query = query.Where(player => player.SteamId == steamId.Value);
    }
    else if (dotaId.HasValue)
    {
      query = query.Where(player => player.DotaId == dotaId.Value);
    }
    else
    {
      return Result<Player>.Failure(PlayerErrors.NoIdentifierProvided);
    }

    var player = await query.FirstOrDefaultAsync();

    return player == null ?
      Result<Player>.Failure(PlayerErrors.NotFound) :
      Result<Player>.Success(player);
  }

  public async Task<Result<PaginatedList<Player>>> GetAllPaginatedListAsync(
    int pageIndex = 1,
    int pageSize = 10
  )
  {
    try
    {
      var query = _context.Players.AsNoTracking();
      var paginatedResult = await PaginatedList<Player>.CreateAsync(query, pageIndex, pageSize);

      return Result<PaginatedList<Player>>.Success(paginatedResult);
    }
    catch
    {
      return Result<PaginatedList<Player>>.Failure(PlayerErrors.RetrieveFailed);
    }
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
      return Result<Player>.Failure(PlayerErrors.AlreadyExists);
    }

    try
    {
      var AddPlayer = new Player()
      {
        DotaId = dotaId,
        SteamId = steamId
      };

      _context.Players.Add(AddPlayer);
      var rowsAffected = await _context.SaveChangesAsync();
      return Result<Player>.Success(AddPlayer);
    }
    catch
    {
      return Result<Player>.Failure(PlayerErrors.CreateFailed);
    }
  }

  public async Task<Result<Player>> UpdatePublicDataAsync(
    bool? isPublicForLadder,
    string? publicName,
    long? id = null,
    long? steamId = null,
    long? dotaId = null
  )
  {
    var query = _context.Players.AsQueryable();

    if (id.HasValue)
    {
      query = query.Where(player => player.Id == id.Value);
    }
    else if (steamId.HasValue)
    {
      query = query.Where(player => player.SteamId == steamId.Value);
    }
    else if (dotaId.HasValue)
    {
      query = query.Where(player => player.DotaId == dotaId.Value);
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

    try
    {
      player.PublicName = publicName ?? player.PublicName;
      player.IsPublicForLadder = isPublicForLadder ?? player.IsPublicForLadder;

      await _context.SaveChangesAsync();

      return Result<Player>.Success(player);
    }
    catch
    {
      return Result<Player>.Failure(PlayerErrors.UpdateFailed);
    }
  }

  public async Task<Result<Player>> ChangeDotaSteamIdsAsync(long id, long newDotaId, long newSteamId)
  {
    var player = await _context.Players.FindAsync(id);

    if (player == null)
    {
      return Result<Player>.Failure(PlayerErrors.NotFound);
    }

    var idExists = await _context.Players
      .AsNoTracking()
      .AnyAsync(
        player => player.DotaId == newDotaId ||
        player.SteamId == newSteamId
      );

    if (idExists)
    {
      return Result<Player>.Failure(PlayerErrors.AlreadyExists);
    }

    try
    {
      player.DotaId = newDotaId;
      player.SteamId = newSteamId;

      await _context.SaveChangesAsync();

      return Result<Player>.Success(player);
    }
    catch
    {
      return Result<Player>.Failure(PlayerErrors.CreateFailed);
    }
  }

  public async Task<Result> DeleteByIdAsync(long id)
  {
    var player = await _context.Players.FindAsync(id);

    if (player == null)
    {
      return Result.Failure(PlayerErrors.NotFound);
    }

    try
    {
      _context.Players.Remove(player);
      await _context.SaveChangesAsync();

      return Result.Success();
    }
    catch
    {
      return Result.Failure(PlayerErrors.DeleteFailed);
    }
  }

  public async Task<Result<BatchDeleteResult>> BatchDeleteAsync(long[] ids)
  {
    if (ids == null || ids.Length == 0)
    {
      return Result<BatchDeleteResult>.Failure(PlayerErrors.NoIdentifierProvided);
    }

    List<long> notFoundIds = [];
    var playersToDelete = new List<Player>();

    foreach (var id in ids)
    {
      var player = await _context.Players.FindAsync(id);

      if (player == null)
      {
        notFoundIds.Add(id);
        continue;
      }

      playersToDelete.Add(player);
    }

    if (playersToDelete.Count == 0)
    {
      return Result<BatchDeleteResult>.Failure(PlayerErrors.NotFound);
    }

    try
    {
      var deletedIds = playersToDelete.Select(player => player.Id).ToList();

      _context.Players.RemoveRange(playersToDelete);
      await _context.SaveChangesAsync();

      return Result<BatchDeleteResult>.Success(new BatchDeleteResult(deletedIds.Count, notFoundIds, deletedIds));
    }
    catch
    {
      return Result<BatchDeleteResult>.Failure(PlayerErrors.DeleteFailed);
    }
  }
}
