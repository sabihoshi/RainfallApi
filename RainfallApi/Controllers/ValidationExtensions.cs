using FluentValidation.Results;
using RainfallApi.Models;

namespace RainfallApi.Controllers;

/// <summary>
///     Extension methods for validations
/// </summary>
public static class ValidationExtensions
{
    /// <summary>
    ///     Converts a ValidationResult to an ErrorResponse
    /// </summary>
    /// <param name="validationResult">The validation result</param>
    /// <param name="message">The message to include in the response</param>
    /// <returns></returns>
    public static ErrorResponse ToErrorResponse(this ValidationResult validationResult, string? message = null) => new()
    {
        Message = message ?? "Validation failed",
        Details = validationResult.Errors.Select(x => new ErrorDetail
        {
            Message      = x.ErrorMessage,
            PropertyName = x.PropertyName
        }).ToList()
    };
}