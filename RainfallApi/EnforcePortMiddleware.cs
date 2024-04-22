using System.Net;
using System.Text.Json;
using RainfallApi.Models;

namespace RainfallApi;

/// <summary>
///     Middleware to enforce a specific port for incoming requests
/// </summary>
public class EnforcePortMiddleware(RequestDelegate next, int correctPort)
{
    /// <summary>
    ///     Invokes the middleware
    /// </summary>
    /// <param name="context">The current HTTP context</param>
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Host.Port != correctPort)
        {
            context.Response.StatusCode = (int) HttpStatusCode.BadGateway;

            var result = new ErrorResponse
            {
                Message = "Bad Gateway. The request was made to a wrong port.",
                Details = []
            };

            var resultJson = JsonSerializer.Serialize(result);
            await context.Response.WriteAsync(resultJson);

            return;
        }

        await next(context);
    }
}