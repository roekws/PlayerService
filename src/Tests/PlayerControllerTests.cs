// using System.Net;
// using Microsoft.AspNetCore.Mvc.Testing;


// namespace Players.IntegrationTests;

// [TestCaseOrderer(
//     ordererTypeName: "XUnit.Project.Orderers.AlphabeticalOrderer",
//     ordererAssemblyName: "XUnit.Project")]
// public class PlayerIntegrationTests(WebApplicationFactory<Program> factory)
//     : IClassFixture<WebApplicationFactory<Program>>
// {
//   private readonly WebApplicationFactory<Program> _factory = factory;

//   [Theory]
//   [InlineData("/api/player/exists")]
//   public async Task GetPlayerByDotaId_WithoutAnyHeaders_ReturnsNotFound(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Get, url);

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
//   }

//   [Theory]
//   [InlineData("/api/player/exists")]
//   public async Task GetPlayerByDotaId_WithoutKeyHeader_ReturnsNotFound(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Get, url);
//     request.Headers.Add("X-Dota-Id", "123");

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
//   }

//   [Theory]
//   [InlineData("/api/player/exists")]
//   public async Task GetPlayerByDotaId_WithoutDotaIdHeader_ReturnsNotFound(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Get, url);
//     request.Headers.Add("X-Dedicated-Server-Key", Environment.GetEnvironmentVariable("API_KEY"));

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
//   }

//   [Theory]
//   [InlineData("/api/player/exists")]
//   public async Task GetPlayerByDotaId_WithInvalidDotaId_ReturnsBadRequest(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Get, url);
//     request.Headers.Add("X-Dota-Id", "IdIs2");
//     request.Headers.Add("X-Dedicated-Server-Key", Environment.GetEnvironmentVariable("API_KEY"));

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//   }

//   [Theory]
//   [InlineData("/api/player")]
//   public async Task CreatePlayer_WithNewDotaId_ReturnsCreated(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Post, url);
//     request.Headers.Add("X-Dota-Id", "11111111");
//     request.Headers.Add("X-Dedicated-Server-Key", Environment.GetEnvironmentVariable("API_KEY"));

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.Created, response.StatusCode);
//   }

//   [Theory]
//   [InlineData("/api/player/exists")]
//   public async Task GetPlayerByDotaId_WithValidHeaders_ReturnsCreated(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Get, url);
//     request.Headers.Add("X-Dota-Id", "11111111");
//     request.Headers.Add("X-Dedicated-Server-Key", Environment.GetEnvironmentVariable("API_KEY"));

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//   }

//   [Theory]
//   [InlineData("/api/player")]
//   public async Task CreatePlayer_WithExistingDotaId_ReturnsBadRequest(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Post, url);
//     request.Headers.Add("X-Dota-Id", "11111111");
//     request.Headers.Add("X-Dedicated-Server-Key", Environment.GetEnvironmentVariable("API_KEY"));

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
//   }

//   [Theory]
//   [InlineData("/api/player")]
//   public async Task DeletePlayer_WithExistingDotaId_ReturnsNoContent(string url)
//   {
//     // Arrange
//     var client = _factory.CreateClient();

//     var request = new HttpRequestMessage(HttpMethod.Delete, url);
//     request.Headers.Add("X-Dota-Id", "11111111");
//     request.Headers.Add("X-Dedicated-Server-Key", Environment.GetEnvironmentVariable("API_KEY"));

//     // Act
//     var response = await client.SendAsync(request);

//     // Assert
//     Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
//   }
// }
