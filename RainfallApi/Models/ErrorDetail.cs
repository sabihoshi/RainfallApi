namespace RainfallApi.Models;

/// <summary>
///     Details of an invalid request property
/// </summary>
public class ErrorDetail
{
    /// <summary>
    ///     The error message associated with the property
    /// </summary>
    public required string Message { get; set; }

    /// <summary>
    ///     The name of the property that caused the error
    /// </summary>
    public required string PropertyName { get; set; }
}