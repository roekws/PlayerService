using Players.Core.Data.Results;
using Players.Core.Entities;

namespace Players.Core.Services;

public interface IPlayerService
{
  Task<Result<Player>> GetAsync(long? id, long? dotaId, long? steamId);
  Task<Result<PaginatedList<Player>>> GetAllPaginatedListAsync(int pageIndex = 1, int pageSize = 10);
  Task<Result<Player>> RegisterAsync(long dotaId, long steamId);
  Task<Result<Player>> UpdatePublicDataAsync(
    bool? isPublicForLadder,
    string? publicName,
    long? playerId = null,
    long? steamId = null,
    long? dotaId = null
  );

  Task<Result<Player>> ChangeDotaSteamIdsAsync(long id, long newDotaId, long newSteamId);
  Task<Result> DeleteByIdAsync(long id);
  Task<Result<BatchDeleteResult>> BatchDeleteAsync(long[] ids);
}
