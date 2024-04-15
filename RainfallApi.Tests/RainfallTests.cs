using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RainfallApi.Client;
using RainfallApi.Client.Models;
using RainfallApi.Controllers.Rainfall;

namespace RainfallApi.Tests;

[TestFixture]
public class RainfallTests : BaseUnitTest<RainfallController>
{
    [Test]
    public async Task GetRainfallReadings_ReturnsOk_WhenValidStationIdAndCountAreProvided()
    {
        // Arrange
        var stationId = "validStationId";
        var count     = 5;

        var response = new RainfallRequestResult
        {
            Context = new Uri("https://environment.data.gov.uk/flood-monitoring/meta/context.jsonld"),
            Meta = new Meta
            {
                Publisher     = "Environment Agency",
                Licence       = new Uri("https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/"),
                Documentation = new Uri("https://environment.data.gov.uk/flood-monitoring/doc/reference"),
                Version       = "0.9",
                Comment       = "Status: Beta service",
                HasFormat     = [],
                Limit         = 100
            },
            Items =
            [
                new Item
                {
                    Id       = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    DateTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
                    Measure  = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    Value    = 0
                }
            ]
        };

        Get<IRainfallApi>()
           .GetRainfallReadingsAsync(Arg.Any<string>(), Arg.Any<int>())
           .Returns(response);

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenNoStationId()
    {
        // Arrange
        var stationId = string.Empty;
        var count     = 5;

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenCountIsLessThanZero()
    {
        // Arrange
        var stationId = "validStationId";
        var count     = -1;

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenCountIsGreaterThanHundred()
    {
        // Arrange
        var stationId = "validStationId";
        var count     = 101;

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }
}