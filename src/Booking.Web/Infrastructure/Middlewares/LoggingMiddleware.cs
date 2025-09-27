namespace Booking.Web.Infrastructure.Middlewares;

public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var requestPath = context.Request.Path;
        var method = context.Request.Method;
        
        logger.LogInformation("Handling HTTP request: {Method} {Path}", method, requestPath);

        await next(context);

        logger.LogInformation("Finished handling HTTP request: {Method} {Path}", method, requestPath);
    }
}