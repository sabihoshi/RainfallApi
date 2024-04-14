using Microsoft.AspNetCore.Mvc;
using RainfallApi.Models;

namespace RainfallApi.Controllers.Rainfall;

[ApiController]
[Route("[controller]")]
public class RainfallController : ControllerBase
{
    /// <summary>
    ///     Retrieve the latest readings for the specified stationId
    /// </summary>
    /// <param name="stationId">The id of the reading station</param>
    /// <param name="count">The number of readings to return</param>
    /// <returns>A list of rainfall readings</returns>
    [HttpGet("id/{stationId}/readings")]
    [ProducesResponseType(typeof(RainfallReadingResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<IActionResult> GetRainfallReadings(string stationId, [FromQuery] int count = 10)
    {
        if (count is < 0 or > 100)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Invalid request",
                Details =
                [
                    new ErrorDetail { Message = "Count must be between 0 and 100", PropertyName = "count" }
                ]
            });
        }

        var api    = Client.RainfallApi.Create();
        var result = await api.GetRainfallReadingsAsync(stationId, count);

        return Ok(new RainfallReadingResponse
        {
            Readings = result.Items.Select(x => new RainfallReading
            {
                DateMeasured   = x.DateTime.DateTime,
                AmountMeasured = x.Value
            }).ToList()
        });
    }
}