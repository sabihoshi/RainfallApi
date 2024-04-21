using OneOf;
using RainfallApi.Models;

namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///     Service for fetching rainfall readings
/// </summary>
public interface IRainfallService
{
    /// <summary>
    ///     Get the latest rainfall readings for a station
    /// </summary>
    /// <param name="stationId">The id of the reading station</param>
    /// <param name="count">The number of readings to return</param>
    /// <returns>A list of rainfall readings</returns>
    Task<OneOf<RainfallReadingResponse, ErrorResponse>> GetRainfallReadingsAsync(
        string stationId, int count);
}