using Players.Core.Data.Pagination;
using Players.Core.Entities;

namespace Players.Core.Services;

public interface IMatchService
{
  Task<Match?> GetByIdAsync(long id, bool detailed);
  Task<Match?> GetActiveByPlayerId(long playerId, bool detailed);
  Task<PaginatedList<Match>> GetPaginatedByPlayerId(
    long playerId,
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  );
  Task<Match?> CreateMatch(long playerId, long gameClientVersion);
}
