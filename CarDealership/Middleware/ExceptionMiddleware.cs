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
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "A aparut o eroare neasteptata." }));
            }
            else
            {
                context.Response.Redirect("/Home/Error");
            }
        }
    }
}