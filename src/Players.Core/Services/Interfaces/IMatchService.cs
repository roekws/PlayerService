using Players.Core.Entities;
using Players.Core.Data.Results;

namespace Players.Core.Services;

public interface IMatchService
{
  Task<Result<Match>> GetByIdAsync(long id, bool detailed);
  Task<Result<Match>> GetActiveByPlayerIdAsync(long playerId, bool detailed);
  Task<PaginatedList<Match>> GetPaginatedByPlayerIdAsync(
    long playerId,
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  );
  Task<Result<Match>> CreateMatchAsync(long playerId, long gameClientVersion);
}
