using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RainfallApi.Tests;

public class RainfallControllerTests : WebApplicationFactory<Program>
{
    private HttpClient _client;

    [Test]
    public async Task GetRainfallReadingsController_ReturnsBadGateway_WhenWrongPortIsUsed()
    {
        // Arrange
        _client.BaseAddress = new Uri("http://localhost:9999");

        // Act
        var response = await _client.GetAsync("/rainfall/id/3680/readings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadGateway);
    }

    [Test]
    public async Task GetRainfallReadingsController_ReturnsBadRequest_WhenCountIsGreaterThanOneHundred()
    {
        // Act
        var response = await _client.GetAsync("/rainfall/id/3680/readings?count=101");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetRainfallReadingsController_ReturnsBadRequest_WhenCountIsLessThanOne()
    {
        // Act
        var response = await _client.GetAsync("/rainfall/id/3680/readings?count=0");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task GetRainfallReadingsController_ReturnsNotFound_WhenNoReadingsAreFound()
    {
        // Act
        var response = await _client.GetAsync("/rainfall/id/9999/readings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task GetRainfallReadingsController_ReturnsOk_WhenValidStationIdAndCountAreProvided()
    {
        // Act
        var response = await _client.GetAsync("/rainfall/id/3680/readings");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TearDown]
    public void OneTimeTearDown() => _client.Dispose();

    [SetUp]
    public void Setup() => _client = CreateClient();
}