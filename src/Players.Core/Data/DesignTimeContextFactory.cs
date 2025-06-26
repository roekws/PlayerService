using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Players.Core.Data;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<PlayerContext>
{
  public PlayerContext CreateDbContext(string[] args)
  {
    // This is only used by EF tools, not in production
    var options = new DbContextOptionsBuilder<PlayerContext>()
      .UseNpgsql("Host=localhost;Port=5432;Database=players;Username=postgres;Password=postgres;")
      .Options;

    return new PlayerContext(options);
  }
}
