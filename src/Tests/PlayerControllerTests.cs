using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PlayerService.Core.Data;
using PlayerService.IntegrationTests.Data;

namespace PlayerService.IntegrationTests;

public class PlayerIntegrationTests
  : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<DatabaseFixture>
{
  private readonly WebApplicationFactory<Program> _factory;
  private readonly DatabaseFixture _dbFixture;

  public PlayerIntegrationTests(WebApplicationFactory<Program> factory, DatabaseFixture dbFixture)
  {
    _factory = factory;
    _dbFixture = dbFixture;

    // Configure factory to use test database
    _factory = _factory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureTestServices(services =>
      {
        // Replace database connection with test container
        services.RemoveAll<DbContextOptions<PlayerContext>>();
        services.AddDbContext<PlayerContext>(options => options.UseNpgsql(_dbFixture.ConnectionString));
      });
    });
  }

  [Theory]
  [InlineData("/api/player/exists")]
  public async Task GetPlayerByDotaId_WithoutAnyHeaders_ReturnsNotFound(string url)
  {
    // Arrange
    var client = _factory.CreateClient();

    var request = new HttpRequestMessage(HttpMethod.Get, url);

    // Act
    var response = await client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Theory]
  [InlineData("/api/player/exists")]
  public async Task GetPlayerByDotaId_WithoutKeyHeader_ReturnsNotFound(string url)
  {
    // Arrange
    var client = _factory.CreateClient();

    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("X-Dota-Id", "123");

    // Act
    var response = await client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Theory]
  [InlineData("/api/player/exists")]
  public async Task GetPlayerByDotaId_WithoutDotaIdHeader_ReturnsNotFound(string url)
  {
    // Arrange
    var client = _factory.CreateClient();

    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("X-Dedicated-Server-Key", "valid-key");

    // Act
    var response = await client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }

  [Theory]
  [InlineData("/api/player/exists")]
  public async Task GetPlayerByDotaId_WithInvalidDotaId_ReturnsBadRequest(string url)
  {
    // Arrange
    var client = _factory.CreateClient();

    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("X-Dota-Id", "IdIs2");
    request.Headers.Add("X-Dedicated-Server-Key", "valid-key");

    // Act
    var response = await client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }

  // Tests are running in alphabetical order
  // Tests must run in order to validate create then fail on duplicate id
  // Use step of 10 in case of needing to insert new test method later
  [Theory]
  [InlineData("/api/player")]
  public async Task A010_CreatePlayer_WithNewDotaId_ReturnsCreated(string url)
  {
    // Arrange
    var client = _factory.CreateClient();

    var request = new HttpRequestMessage(HttpMethod.Post, url);
    request.Headers.Add("X-Dota-Id", "1");
    request.Headers.Add("X-Dedicated-Server-Key", "valid-key");

    // Act
    var response = await client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
  }

  [Theory]
  [InlineData("/api/player/exists")]
  public async Task A020_GetPlayerByDotaId_WithValidHeaders_ReturnsCreated(string url)
  {
    // Arrange
    var client = _factory.CreateClient();

    var request = new HttpRequestMessage(HttpMethod.Get, url);
    request.Headers.Add("X-Dota-Id", "1");
    request.Headers.Add("X-Dedicated-Server-Key", "valid-key");

    // Act
    var response = await client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Theory]
  [InlineData("/api/player")]
  public async Task A030_CreatePlayer_WithExistingDotaId_ReturnsBadRequest(string url)
  {
    // Arrange
    var client = _factory.CreateClient();

    var request = new HttpRequestMessage(HttpMethod.Post, url);
    request.Headers.Add("X-Dota-Id", "1");
    request.Headers.Add("X-Dedicated-Server-Key", "valid-key");

    // Act
    var response = await client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
  }
}
