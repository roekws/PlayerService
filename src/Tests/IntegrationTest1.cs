using Aspire.Hosting;
using Microsoft.Extensions.Logging;

namespace Tests.Tests;

public class IntegrationTest1
{

  [Fact]
  public async Task GetWebResourceRootReturnsOkStatusCode()
  {
    // Arrange
    var builder = await DistributedApplicationTestingBuilder
      .CreateAsync<Projects.Players_AppHost>();

    var api = builder.CreateResourceBuilder<ProjectResource>("apiservice");
    var envVars = await api.Resource.GetEnvironmentVariableValuesAsync(DistributedApplicationOperation.Publish);

    await using var app = await builder.BuildAsync();
    await app.StartAsync();

    // Act
    var httpClient = app.CreateHttpClient("apiservice");

    await app.ResourceNotifications.WaitForResourceHealthyAsync(
        "apiservice");

    var response = await httpClient.GetAsync("/ ");

    // Assert
  }
}
