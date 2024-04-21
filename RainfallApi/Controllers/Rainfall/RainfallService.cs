using System.Net;
using OneOf;
using RainfallApi.Client;
using RainfallApi.Models;
using Refit;

namespace RainfallApi.Controllers.Rainfall;

/// <inheritdoc />
public class RainfallService(IRainfallApi rainfallApi) : IRainfallService
{
    /// <inheritdoc />
    public async Task<OneOf<RainfallReadingResponse, ErrorResponse>> GetRainfallReadingsAsync(
        string stationId, int count)
    {
        var query = new RainfallReadingQuery(stationId, count);
        var validationResult = await new RainfallReadingQueryValidator().ValidateAsync(query);
        if (!validationResult.IsValid) return validationResult.ToErrorResponse();

        try
        {
            var result = await rainfallApi.GetRainfallReadingsAsync(stationId, count);
            if (result.Items.Length == 0)
            {
                return new ErrorResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "No readings found for the specified stationId",
                    Details =
                    [
                        new ErrorDetail
                        {
                            Message = "No readings found for the specified stationId",
                            PropertyName = nameof(stationId)
                        }
                    ]
                };
            }

            return new RainfallReadingResponse
            {
                Readings = result.Items.Select(x => new RainfallReading
                {
                    DateMeasured = x.DateTime.DateTime,
                    AmountMeasured = x.Value
                }).ToList()
            };
        }
        catch (ApiException e)
        {
            return await e.ToErrorResponseAsync();
        }
        catch (HttpRequestException e)
        {
            return e.ToErrorResponse();
        }
    }
}