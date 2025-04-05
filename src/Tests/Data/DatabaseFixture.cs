using Microsoft.EntityFrameworkCore;
using PlayerService.Core.Data;
using Testcontainers.PostgreSql;

namespace PlayerService.IntegrationTests.Data;

public sealed class DatabaseFixture : IAsyncLifetime
{
  private readonly PostgreSqlContainer _dbContainer =
    new PostgreSqlBuilder()
      .WithImage("postgres:16-alpine")
      .WithDatabase("PlayerServiceDb")
      .WithUsername("postgres")
      .WithPassword("postgres")
      .WithCleanUp(true)
      .Build();

  public string ConnectionString => _dbContainer.GetConnectionString();

  public async Task InitializeAsync()
  {
    await _dbContainer.StartAsync();

    // Apply migrations
    var options = new DbContextOptionsBuilder<PlayerContext>()
        .UseNpgsql(_dbContainer.GetConnectionString())
        .Options;

    using var context = new PlayerContext(options);
    await context.Database.MigrateAsync();
  }

  public async Task DisposeAsync()
  {
    await _dbContainer.DisposeAsync();
  }
}
