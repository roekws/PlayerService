using Microsoft.EntityFrameworkCore;

namespace Players.Core.Data.Pagination;

public class PaginatedList<T>
{
  private PaginatedList(List<T> items, int pageIndex, int pageSize, int totalCount)
  {
    Items = items;
    PageIndex = pageIndex;
    PageSize = pageSize;
    TotalCount = totalCount;
  }

  public PaginatedList()
  {

  }

  private const int MaxPageSize = 100;

  public List<T> Items { get; set; } = [];

  public int PageIndex { get; set; }

  public int PageSize { get; set; }

  public int TotalCount { get; set; }

  public bool HasNextPage => PageIndex * PageSize < TotalCount;

  public bool HasPreviousPage => PageIndex > 1;

  public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
  {
    pageSize = pageSize > MaxPageSize || pageSize < 0 ? MaxPageSize : pageSize;
    pageIndex = pageIndex > 0 ? pageIndex : 1;

    var totalCount = await source.CountAsync();
    var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

    return new PaginatedList<T>(items, pageIndex, pageSize, totalCount);
  }
}
