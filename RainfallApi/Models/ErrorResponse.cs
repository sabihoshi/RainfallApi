using System.Net;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace RainfallApi.Models;

/// <summary>
///     An error object returned for failed requests
/// </summary>
[SwaggerSchema(Title = "Error response")]
public class ErrorResponse
{
    /// <summary>
    ///     The HTTP status code of the response
    /// </summary>
    [JsonIgnore]
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    ///     Specific details about what caused the error
    /// </summary>
    public required List<ErrorDetail> Details { get; set; } = [];

    /// <summary>
    ///     General error message
    /// </summary>
    public required string Message { get; set; }
}