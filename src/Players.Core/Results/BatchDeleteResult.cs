using System.Runtime.InteropServices;
using Players.Core.Data.Errors;

namespace Players.Core.Data.Results;

public class BatchDeleteResult
{
  public int DeletedCount { get; set; }
  public List<long> NotFoundIds { get; set; }
  public List<long> DeletedIds { get; set; }

  public BatchDeleteResult(int deletedCount, List<long> notFoundIds, List<long> deletedIds)
  {
    DeletedCount = deletedCount;
    NotFoundIds = notFoundIds;
    DeletedIds = deletedIds;
  }
}
