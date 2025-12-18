using Booking.Infrastructure.Telemetry;
using BookingApp.Abstractions;
using BookingApp.BookingAggregate.Services;
using BookingApp.DomainServices.Bookings;
using BookingApp.ExternalConfigs;
using BookingApp.FeatureToggles;
using BookingApp.Messaging;
using BookingApp.Persistence;
using Microsoft.Extensions.DependencyInjection;
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
        builder.AddDomainServices();
        builder.AddTelemetry();

        builder.Services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(typeof(IApplicationMarker).Assembly));


        builder.Services.AddSingleton(TimeProvider.System);
        
        return builder;
    }

    private static IHostApplicationBuilder AddDomainServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBookingPolicyService, BookingPolicyService>();
        return builder;
    }
}