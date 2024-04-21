using AutoFixture;
using FluentAssertions;
using FluentAssertions.OneOf;
using NSubstitute;
using RainfallApi.Client;
using RainfallApi.Client.Models;
using RainfallApi.Controllers.Rainfall;
using RainfallApi.Models;

namespace RainfallApi.Tests;

[TestFixture]
public class RainfallTests : BaseUnitTest<RainfallService>
{
    [Test]
    public async Task GetRainfallReadings_ReturnsOk_WhenValidStationIdAndCountAreProvided()
    {
        // Arrange
        var stationId = "validStationId";
        var count = 1;

        var response = Fixture.Build<RainfallRequestResult>()
           .With(r => r.Items, [
                new Item
                {
                    Id = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    DateTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
                    Measure = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    Value = 0
                }
            ])
           .Create();

        Get<IRainfallApi>()
           .GetRainfallReadingsAsync(Arg.Any<string>(), Arg.Any<int>())
           .Returns(response);

        // Act
        var result = await Unit.GetRainfallReadingsAsync(stationId, count);

        // Assert
        result.Should().BeCase<RainfallReadingResponse>();
        result.Value.As<RainfallReadingResponse>().Readings.Should().HaveCount(1);
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
                    Id = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    DateTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z"),
                    Measure = new Uri("https://environment.data.gov.uk/flood-monitoring/"),
                    Value = 0
                }
            })
           .Create();

        Get<IRainfallApi>()
           .GetRainfallReadingsAsync(Arg.Any<string>(), Arg.Any<int>())
           .Returns(response);

        // Act
        var result = await Unit.GetRainfallReadingsAsync(stationId, count);

        // Assert
        result.Should().BeCase<RainfallReadingResponse>();
        result.Value.Should().BeOfType<RainfallReadingResponse>();
        result.Value.As<RainfallReadingResponse>().Readings.Should().HaveCount(1);
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenNoStationId()
    {
        // Arrange
        var stationId = string.Empty;
        var count = 5;

        // Act
        var result = await Unit.GetRainfallReadingsAsync(stationId, count);

        // Assert
        result.Should().BeCase<ErrorResponse>();
        result.Value.Should().BeOfType<ErrorResponse>();
        result.Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain(nameof(RainfallReadingQuery.StationId));
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenCountIsLessThanOne()
    {
        // Arrange
        var stationId = "validStationId";
        var count = 0;

        // Act
        var result = await Unit.GetRainfallReadingsAsync(stationId, count);

        // Assert
        result.Should().BeCase<ErrorResponse>();
        result.Value.Should().BeOfType<ErrorResponse>();
        result.Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain(nameof(RainfallReadingQuery.Count));
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsBadRequest_WhenCountIsGreaterThanHundred()
    {
        // Arrange
        var stationId = "validStationId";
        var count = 101;

        // Act
        var result = await Unit.GetRainfallReadingsAsync(stationId, count);

        // Assert
        result.Should().BeCase<ErrorResponse>();
        result.Value.Should().BeOfType<ErrorResponse>();
        result.Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain(nameof(RainfallReadingQuery.Count));
    }

    [Test]
    public async Task GetRainfallReadings_ReturnsNotFound_WhenNoReadingsAreFound()
    {
        // Arrange
        var stationId = "validStationId";
        var count = 1;

        var response = Fixture.Build<RainfallRequestResult>()
           .With(r => r.Items, [])
           .Create();

        Get<IRainfallApi>()
           .GetRainfallReadingsAsync(Arg.Any<string>(), Arg.Any<int>())
           .Returns(response);

        // Act
        var result = await Unit.GetRainfallReadingsAsync(stationId, count);

        // Assert
        result.Should().BeCase<ErrorResponse>();
        result.Value.Should().BeOfType<ErrorResponse>();
        result.Value
           .As<ErrorResponse>().Details
           .Select(x => x.PropertyName)
           .Should().Contain("stationId");
    }
}