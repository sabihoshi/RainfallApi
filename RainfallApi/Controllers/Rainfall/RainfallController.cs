using Microsoft.AspNetCore.Mvc;
using RainfallApi.Client;
using RainfallApi.Models;

namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///     Controller for retrieving rainfall readings
/// </summary>
[ApiController]
[Route("[controller]")]
public class RainfallController(IRainfallApi rainfallApi) : ControllerBase
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
        var query           = new RainfallReadingQuery(stationId, count);
        var queryValidation = await new RainfallReadingQueryValidator().ValidateAsync(query);
        if (!queryValidation.IsValid) return BadRequest(queryValidation.ToErrorResponse());

        var result           = await rainfallApi.GetRainfallReadingsAsync(stationId, count);
        var resultValidation = await new RainfallRequestResultValidator().ValidateAsync(result);
        if (!resultValidation.IsValid) return NotFound(resultValidation.ToErrorResponse());

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