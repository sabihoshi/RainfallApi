using System.Net;
using System.Text.Json;
using RainfallApi.Models;

namespace RainfallApi;

/// <summary>
///     Middleware for handling exceptions
/// </summary>
public abstract class AbstractExceptionMiddleware(RequestDelegate next)
{
    /// <summary>
    ///     Invokes the request and catches any exceptions
    /// </summary>
    /// <param name="context">The HTTP context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    ///     Gets HTTP status code response and message to be returned to the caller.
    /// </summary>
    /// <param name="exception">The actual exception</param>
    /// <returns>A response of <see cref="ErrorResponse" /></returns>
    protected abstract Task<ErrorResponse> GetResponseAsync(Exception exception);

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        var response = await GetResponseAsync(exception);
        var result = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(result);
    }
}