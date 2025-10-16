using BookingApp.ExternalConfigs;
using BookingApp.FeatureToggles;
using BookingApp.Messaging;
using BookingApp.Persistence;
using Microsoft.Extensions.Hosting;

namespace BookingApp;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMq();
        builder.AddExternalConfigurations();
        builder.AddFeatureToggles();
        builder.AddPersistence();
        
        return builder;
    }
}