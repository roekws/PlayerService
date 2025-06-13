using Players.Core.Data.Results;
using Players.Core.Entities;

namespace Players.Core.Services;

public interface IPlayerService
{
  Task<Result<Player>> GetByIdAsync(long id);
  Task<Result<Player>> GetByDotaIdAsync(long dotaId);
  Task<Result<Player>> GetBySteamIdAsync(long steamId);
  Task<Result<Player>> GetByDotaSteamIdsAsync(long dotaId, long steamId);
  Task<PaginatedList<Player>> GetAllPaginatedList(int pageIndex = 1, int pageSize = 10);
  Task<Result<Player>> RegisterAsync(long dotaId, long steamId);
  Task<Result<Player>> UpdatePublicDataAsync(
    bool? isPublicForLadder,
    string? publicName,
    long? playerId = null,
    long? steamId = null,
    long? dotaId = null
  );

  Task<Result<Player>> ChangeDotaSteamIds(long id, long newDotaId, long newSteamId);
  Task<Result> DeleteByIdAsync(long id);
}
