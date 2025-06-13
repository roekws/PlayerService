using Players.Core.Data.Results;
using Players.Core.Entities;

namespace Players.Core.Services;

public interface IPlayerService
{
  Task<Player?> GetByIdAsync(long id);
  Task<Player?> GetByDotaIdAsync(long dotaId);
  Task<Player?> GetBySteamIdAsync(long steamId);
  Task<Player?> GetByDotaSteamIdsAsync(long dotaId, long steamId);
  Task<PaginatedList<Player>> GetAllPaginatedList(int pageIndex = 1, int pageSize = 10);
  Task<Player?> RegisterAsync(long dotaId, long steamId);
  Task<Player?> UpdatePublicDataAsync(
    bool? isPublicForLadder,
    string? publicName,
    long? playerId = null,
    long? steamId = null,
    long? dotaId = null
  );

  Task<Player?> ChangeDotaSteamIds(long id, long newDotaId, long newSteamId);
  Task<bool?> DeleteByIdAsync(long id);
}
