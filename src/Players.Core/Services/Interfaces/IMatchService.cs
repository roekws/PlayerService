using Players.Core.Data.Pagination;
using Players.Core.Entities;

namespace Players.Core.Services;

public interface IMatchService
{
  Task<Match?> GetByIdAsync(long id, bool detailed);
  Task<Match?> GetActiveByPlayerIdAsync(long playerId, bool detailed);
  Task<PaginatedList<Match>> GetPaginatedByPlayerIdAsync(
    long playerId,
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  );
  Task<Match?> CreateMatchAsync(long playerId, long gameClientVersion);
}
