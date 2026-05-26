using System.Net;
using System.Text.Json;

namespace CarDealership.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        try { await _next(context); }
        catch (Exception ex)
        {
            logger.LogError(ex, "A aparut o eroare neasteptata.");
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "A aparut o eroare neasteptata." }));
        }
    }
}