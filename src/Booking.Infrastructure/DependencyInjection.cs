using Booking.Infrastructure.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        return builder;
    }

    private static IHostApplicationBuilder AddRabbitMq(this IHostApplicationBuilder builder)
    {
        var config = builder.Configuration.GetSection(RabbitMQSettings.Section);
        var settings = new RabbitMQSettings();
        config.Bind(settings);

        builder.Services.AddSingleton(settings);
        
        
        
        return builder;
    }
}