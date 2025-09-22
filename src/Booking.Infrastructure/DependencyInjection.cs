using Booking.Core.Messaging;
using Booking.Infrastructure.Messaging;
using Booking.Infrastructure.Messaging.Producer;
using Booking.Infrastructure.Messaging.Subscriber;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMq();
        return builder;
    }

    private static IHostApplicationBuilder AddRabbitMq(this IHostApplicationBuilder builder)
    {
        var config = builder.Configuration.GetSection(RabbitMQSettings.Section);
        var settings = new RabbitMQSettings();
        config.Bind(settings);

        builder.Services.AddSingleton(settings);

        builder.Services.AddSingleton<ModelFactory>();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<ModelFactory>().CreateChannel());

        builder.Services.AddSingleton<RabbitMqReceiver>();
        builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
        
        //todo: register event listeners here

        builder.Services.AddHostedService<WorkerService>();
        
        return builder;
    }
}