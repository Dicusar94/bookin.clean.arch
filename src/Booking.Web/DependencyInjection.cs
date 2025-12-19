using Asp.Versioning;
using BookingApp.FeatureToggles.Services;
using BookingApp.Infrastructure.Middlewares;
using BookingApp.Infrastructure.SwaggerConfigs;
using Microsoft.FeatureManagement;

namespace BookingApp;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddPresentation(this IHostApplicationBuilder builder)
    {
        builder.ConfigureApi();
        builder.ConfigureSwagger();
        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<ITestService, TestServiceOne>();
        builder.Services.AddSingleton<ITestService, TestServiceTwo>();
        
        return builder.AddInfrastructure();
    }

    public static IApplicationBuilder UseMiddlewares(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ActivityTracingMiddleware>();
        applicationBuilder.UseMiddlewareForFeature<LoggingMiddleware>("logging-middleware");
        return applicationBuilder;
    }

    private static IHostApplicationBuilder ConfigureApi(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddEndpointsApiExplorer()
            .AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1.0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
                config.ReportApiVersions = true;
            })
            .AddApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            })
            .EnableApiVersionBinding();

        return builder;
    }

}