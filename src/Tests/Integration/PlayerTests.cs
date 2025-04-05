using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using PlayerService.Core.Data;

namespace PlayerService.IntegrationTests;

public class PlayerTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> _factory = factory;

  [Theory]
  [InlineData("/api/player/exists")]
  public async Task Endpoints_WhenHeadersMissing_ReturnsNotFound(string url)
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
  public async Task Endpoints_WhenKeyHeaderMissing_ReturnsNotFound(string url)
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
  public async Task Endpoints_WhenDotaIdHeaderMissing_ReturnsNotFound(string url)
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
  public async Task Endpoints_WhenDotaIdNotValid_ReturnsBadRequest(string url)
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
}
