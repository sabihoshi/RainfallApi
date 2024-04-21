using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using RainfallApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RainfallApi.Controllers.Rainfall;

/// <summary>
///     Controller for retrieving rainfall readings
/// </summary>
[ApiController]
[Route("rainfall")]
[SwaggerTag("Operations relating to rainfall")]
public class RainfallController(IRainfallService rainfallService) : ControllerBase
{
    /// <summary>
    ///     Get rainfall readings by station Id
    /// </summary>
    /// <remarks>
    ///     Retrieve the latest readings for the specified stationId
    /// </remarks>
    /// <param name="stationId">The id of the reading station</param>
    /// <param name="count">The number of readings to return</param>
    /// <returns>A list of rainfall readings</returns>
    [HttpGet("id/{stationId}/readings")]
    [SwaggerOperation(OperationId = "get-rainfall")]
    [SwaggerResponse(200, "A list of rainfall readings successfully retrieved", typeof(RainfallReadingResponse), ["application/json"])]
    [SwaggerResponse(400, "Invalid request", typeof(ErrorResponse), ["application/json"])]
    [SwaggerResponse(404, "No readings found for the specified stationId", typeof(ErrorResponse), ["application/json"])]
    [SwaggerResponse(500, "Internal server error", typeof(ErrorResponse), ["application/json"])]
    public async Task<IActionResult> GetRainfallReadings(string stationId, [FromQuery] [Range(1, 100)] int count = 10)
    {
        var response = await rainfallService.GetRainfallReadingsAsync(stationId, count);
        return response.Match(Ok, errorResponse => errorResponse.ToActionResult());
    }
}