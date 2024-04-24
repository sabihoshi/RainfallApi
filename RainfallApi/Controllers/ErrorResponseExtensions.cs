using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using RainfallApi.Models;
using Refit;

namespace RainfallApi.Controllers;

/// <summary>
///     Extension methods for validations
/// </summary>
public static class ErrorResponseExtensions
{
    /// <summary>
    ///     Converts a ValidationResult to an ErrorResponse
    /// </summary>
    /// <param name="validationResult">The validation result</param>
    /// <param name="message">The message to include in the response</param>
    /// <returns>An <see cref="ErrorResponse" /></returns>
    public static ErrorResponse ToErrorResponse(this ValidationResult validationResult, string? message = null) => new()
    {
        StatusCode = HttpStatusCode.BadRequest,
        Message = message ?? "Validation failed",
        Details = validationResult.Errors.Select(x => new ErrorDetail
        {
            Message = x.ErrorMessage,
            PropertyName = x.PropertyName
        }).ToList()
    };

    /// <summary>
    ///     Converts a ValidationException to an ErrorResponse
    /// </summary>
    /// <param name="validationException">The validation exception</param>
    /// <returns>An <see cref="ErrorResponse" /></returns>
    public static ErrorResponse ToErrorResponse(this ValidationException validationException) => new()
    {
        StatusCode = HttpStatusCode.BadRequest,
        Message = validationException.Message,
        Details = validationException.Errors.Select(x => new ErrorDetail
        {
            Message = x.ErrorMessage,
            PropertyName = x.PropertyName
        }).ToList()
    };

    /// <summary>
    ///     Converts an HttpRequestException to an ErrorResponse
    /// </summary>
    /// <param name="httpRequestException">The HTTP request exception</param>
    /// <returns>An <see cref="ErrorResponse" /></returns>
    public static ErrorResponse ToErrorResponse(this HttpRequestException httpRequestException) => new()
    {
        StatusCode = HttpStatusCode.InternalServerError,
        Message = "An error occurred with the request to the API",
        Details =
        [
            new ErrorDetail
            {
                Message = httpRequestException.Message,
                PropertyName = string.Empty
            }
        ]
    };

    /// <summary>
    ///     Converts an Exception to an ErrorResponse
    /// </summary>
    /// <param name="exception">The exception</param>
    /// <returns>An <see cref="ErrorResponse" /></returns>
    public static ErrorResponse ToErrorResponse(this Exception exception) => new()
    {
        StatusCode = HttpStatusCode.InternalServerError,
        Message = "An error occurred with the request to the API",
        Details =
        [
            new ErrorDetail
            {
                Message = exception.Message,
                PropertyName = string.Empty
            }
        ]
    };

    /// <summary>
    ///     Converts an ErrorResponse to an IActionResult
    /// </summary>
    /// <param name="errorResponse">The error response</param>
    /// <returns>An <see cref="IActionResult" /></returns>
    public static IActionResult ToActionResult(this ErrorResponse errorResponse) => errorResponse.StatusCode switch
    {
        HttpStatusCode.BadRequest => new BadRequestObjectResult(errorResponse),
        HttpStatusCode.NotFound   => new NotFoundObjectResult(errorResponse),
        _                         => new ObjectResult(errorResponse) { StatusCode = (int) errorResponse.StatusCode }
    };

    /// <summary>
    ///     Converts an ApiException to an ErrorResponse
    /// </summary>
    /// <param name="apiException">The API exception</param>
    /// <returns>An <see cref="ErrorResponse" /></returns>
    public static async Task<ErrorResponse> ToErrorResponseAsync(this ApiException apiException)
    {
        var errors = await apiException.GetContentAsAsync<Dictionary<string, string>>();
        return new ErrorResponse
        {
            StatusCode = apiException.StatusCode,
            Message = apiException.Message,
            Details = errors?.Select(x => new ErrorDetail
            {
                Message = x.Value,
                PropertyName = x.Key
            }).ToList() ?? []
        };
    }
}