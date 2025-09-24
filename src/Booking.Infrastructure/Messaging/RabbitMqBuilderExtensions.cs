using Booking.Application;
using Booking.Core.Messaging;
using Booking.Infrastructure.Messaging.Producer;
using Booking.Infrastructure.Messaging.Subscriber;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

namespace Booking.Infrastructure.Messaging;

public static class RabbitMqBuilderExtensions
{
    public static IHostApplicationBuilder AddRabbitMq(this IHostApplicationBuilder builder)
    {
        var config = builder.Configuration.GetSection(RabbitMQSettings.Section);
        var settings = new RabbitMQSettings();
        config.Bind(settings);

        builder.Services.AddSingleton(settings);

        builder.Services.AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
        {
            DispatchConsumersAsync = true,
            Uri = new Uri(settings.ConnectionString)
            
            // Depending on the personal preference you can use either the username/password approach
            // or via connection string
            //HostName = settings.HostName,
            //UserName = settings.UserName,
            //Password = settings.Password
        });

        builder.Services.AddSingleton<ModelFactory>();
        builder.Services.AddSingleton(sp => sp.GetRequiredService<ModelFactory>().CreateChannel());

        builder.Services.AddSingleton<RabbitMqReceiver>();
        builder.Services.AddSingleton<IMessageProducer, MessageProducer>();
        
        //todo: register event listeners here
        builder.Services.AddSingleton<IListener, TestListener>();

        builder.Services.AddHostedService<WorkerService>();
        
        return builder;
    }
}