using FluentValidation;
using RainfallApi.Models;
using Refit;

namespace RainfallApi.Controllers.Rainfall;

/// <inheritdoc />
public class RainfallServiceExceptionMiddleware(RequestDelegate next) : AbstractExceptionMiddleware(next)
{
    /// <inheritdoc />
    protected override Task<ErrorResponse> GetResponseAsync(Exception exception) => exception switch
    {
        ApiException apiException => apiException.ToErrorResponseAsync(),
        HttpRequestException httpException => Task.FromResult(httpException.ToErrorResponse()),
        ValidationException validationException => Task.FromResult(validationException.ToErrorResponse()),
        _ => Task.FromResult(exception.ToErrorResponse())
    };
}