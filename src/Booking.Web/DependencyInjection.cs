using BookingApp.FeatureToggles.Services;
using BookingApp.Infrastructure.Middlewares;
using Microsoft.FeatureManagement;

namespace BookingApp;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddWeb(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOpenApi();
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSingleton<ITestService, TestServiceOne>();
        builder.Services.AddSingleton<ITestService, TestServiceTwo>();
        
        return builder.AddInfrastructure();
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddlewareForFeature<LoggingMiddleware>("logging-middleware");
        return applicationBuilder;
    }
}