namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///     The query parameters for a rainfall reading request
/// </summary>
/// <param name="StationId">The id of the reading station</param>
/// <param name="Count">The number of readings to return</param>
public record RainfallReadingQuery(string StationId, int Count);