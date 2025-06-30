using Players.Core.Entities;
using Players.Core.Data.Results;
using Players.Core.Enums;

namespace Players.Core.Services;

public interface IMatchService
{
  Task<Result<Match>> GetByIdAsync(long id, bool detailed);
  Task<Result<Match>> GetActiveByPlayerAsync(long dotaId, long steamId, bool detailed, long globalPatchVersion);
  Task<Result<PaginatedList<Match>>> GetPaginatedByPlayerAsync(
    long? dotaId,
    long? steamId,
    long? id,
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  );
  Task<Result<PaginatedList<Match>>> GetPaginatedAllAsync(
    bool detailed,
    int pageIndex = 1,
    int pageSize = 10
  );
  Task<Result<Match>> CreateMatchAsync(long dotaId, long steamId, long globalPatchVersion);
  Task<Result<Character>> CreateCharacterAsync(long id, Hero hero);
}
