using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace IntegrationTests.MockHttpWithWireMock;

public class IntegrationTestsAppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var wireMockServer = WireMockServer.Start();

        builder.ConfigureAppConfiguration(configBuilder =>
        {
            configBuilder.AddInMemoryCollection(new KeyValuePair<string, string>[] {
                new("weatherProviderBaseUrl", wireMockServer.Urls[0])
            });
        }).ConfigureServices(collection => collection.AddSingleton(wireMockServer));
    }

}

public class WeatherControllerTests : IClassFixture<IntegrationTestsAppFactory<Program>>, IAsyncLifetime
{
    private readonly IntegrationTestsAppFactory<Program> _factory;
    private readonly WireMockServer _wireMockServer;

    public WeatherControllerTests(
        IntegrationTestsAppFactory<Program> factory)
    {
        _factory = factory;
        _wireMockServer = factory.Services.GetRequiredService<WireMockServer>();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _wireMockServer.Stop();
        _wireMockServer.Dispose();
        return Task.CompletedTask;
    }

    [Fact]
    public async Task GetTemperature()
    {
        // Arrange
        _wireMockServer
            .Given(Request.Create().WithPath("/temperature-in-spain").UsingGet())
            .RespondWith(Response.Create().WithBody("25").WithStatusCode(HttpStatusCode.OK));

        // Act
        var response = await _factory.CreateClient().GetStringAsync("/weather/temperature");

        // Assert
        Assert.Equal("25", response);
    }

}