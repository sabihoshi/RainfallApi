using RainfallApi.Client.Models;
using Refit;

namespace RainfallApi.Client;

public interface IRainfallApi
{
    /// <summary>
    ///     All readings for measures from a particular station
    /// </summary>
    /// <param name="id">The id of the station</param>
    /// <param name="limit">The number of readings to return</param>
    /// <returns></returns>
    [Get("/id/stations/{id}/readings?_sorted=true")]
    Task<RainfallRequestResult> GetRainfallReadingsAsync(
        string id, [AliasAs("_limit")] int limit = 10);
}