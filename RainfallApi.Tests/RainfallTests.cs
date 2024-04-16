using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using RainfallApi.Client;
using RainfallApi.Client.Models;
using RainfallApi.Controllers.Rainfall;
using RainfallApi.Models;

namespace RainfallApi.Tests;

[TestFixture]
public class RainfallTests : BaseUnitTest<RainfallController>
{
    [Test]
    public async Task GetRainfallReadings_ReturnsOk_WhenValidStationIdAndCountAreProvided()
    {
        // Arrange
        var stationId = "validStationId";
        var count     = 1;

        var response = Fixture.Build<RainfallRequestResult>()
           .With(r => r.Items, [
                new Item
                {
                    Id       = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    DateTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
                    Measure  = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    Value    = 0
                }
            ])
           .Create();

        Get<IRainfallApi>()
           .GetRainfallReadingsAsync(Arg.Any<string>(), Arg.Any<int>())
           .Returns(response);

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeOfType<RainfallReadingResponse>();
        result.As<OkObjectResult>().Value.As<RainfallReadingResponse>().Readings.Should().HaveCount(1);
    }

    [TestCase(1)]
    [TestCase(5)]
    [TestCase(10)]
    [TestCase(50)]
    [TestCase(99)]
    [TestCase(100)]
    public async Task GetRainfallReadings_ReturnsOk_WhenValidStationIdAndCountsAreProvided(int count)
    {
        // Arrange
        var stationId = "validStationId";

        var response = Fixture.Build<RainfallRequestResult>()
           .With(r => r.Items, new[]
            {
                new Item
                {
                    Id       = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    DateTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
                    Measure  = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    Value    = 0
                }
            })
           .Create();

        Get<IRainfallApi>()
           .GetRainfallReadingsAsync(Arg.Any<string>(), Arg.Any<int>())
           .Returns(response);

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeOfType<RainfallReadingResponse>();
        result.As<OkObjectResult>().Value.As<RainfallReadingResponse>().Readings.Should().HaveCount(1);
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
        result.Should().BeOfType<BadRequestObjectResult>();
        result.As<BadRequestObjectResult>().Value.Should().BeOfType<ErrorResponse>();
        result.As<BadRequestObjectResult>().Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain(nameof(RainfallReadingQuery.StationId));
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenCountIsLessThanOne()
    {
        // Arrange
        var stationId = "validStationId";
        var count     = 0;

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        result.As<BadRequestObjectResult>().Value.Should().BeOfType<ErrorResponse>();
        result.As<BadRequestObjectResult>().Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain(nameof(RainfallReadingQuery.Count));
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
        result.Should().BeOfType<BadRequestObjectResult>();
        result.As<BadRequestObjectResult>().Value.Should().BeOfType<ErrorResponse>();
        result.As<BadRequestObjectResult>().Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain(nameof(RainfallReadingQuery.Count));
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsNotFound_WhenNoReadingsAreFound()
    {
        // Arrange
        var stationId = "validStationId";
        var count     = 1;

        var response = Fixture.Build<RainfallRequestResult>()
           .With(r => r.Items, [])
           .Create();

        Get<IRainfallApi>()
           .GetRainfallReadingsAsync(Arg.Any<string>(), Arg.Any<int>())
           .Returns(response);

        // Act
        var result = await Unit.GetRainfallReadings(stationId, count);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        result.As<NotFoundObjectResult>().Value.Should().BeOfType<ErrorResponse>();
        result.As<NotFoundObjectResult>().Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain(nameof(RainfallReadingQuery.StationId));
    }
}