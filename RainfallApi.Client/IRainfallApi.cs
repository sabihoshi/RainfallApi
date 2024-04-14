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
    [Get("/id/stations/{id}/readings")]
    Task<RainfallRequestResult> GetRainfallReadingsAsync(
        string id, [AliasAs("_limit")] int limit = 10);
}

public class RainfallApi
{
    public static IRainfallApi Create()
        => RestService.For<IRainfallApi>("https://environment.data.gov.uk/flood-monitoring");
}