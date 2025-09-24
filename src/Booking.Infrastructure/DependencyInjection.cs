using System.Runtime.Serialization;
using Booking.Application;
using Booking.Core.Messaging;
using Booking.Infrastructure.ExternalConfigs;
using Booking.Infrastructure.Messaging;
using Booking.Infrastructure.Messaging.Producer;
using Booking.Infrastructure.Messaging.Subscriber;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Booking.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddRabbitMq();
        builder.AddExternalConfigurations();
        
        return builder;
    }
}