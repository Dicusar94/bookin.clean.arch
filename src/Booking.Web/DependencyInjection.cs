using Booking.Infrastructure;
using Booking.Web.Infrastructure.Middlewares;
using Microsoft.FeatureManagement;

namespace Booking.Web;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWeb(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        
        return builder.AddInfrastructure();
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddlewareForFeature<LoggingMiddleware>("logging-middleware");
        return applicationBuilder;
    }
}