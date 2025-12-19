using System.Diagnostics;
using BookingApp.Telemetry;

namespace BookingApp.Infrastructure.Middlewares;

public class ActivityTracingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        using var activity = RunTimeDiagnosticConfig.Source.StartActivity(
            name: $"Handling HTTP request: {context.Request.Method} {context.Request.Path}",
            ActivityKind.Server);

        activity?.SetTag("http.method", context.Request.Method);
        activity?.SetTag("http.url", context.Request.Path);

        await next(context);
    }
}