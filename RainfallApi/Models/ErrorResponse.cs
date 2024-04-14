namespace RainfallApi.Models;

/// <summary>
///     An error object returned for failed requests
/// </summary>
public class ErrorResponse
{
    /// <summary>
    ///     Specific details about what caused the error
    /// </summary>
    public required List<ErrorDetail> Details { get; set; } = [];

    /// <summary>
    ///     General error message
    /// </summary>
    public required string Message { get; set; }
}