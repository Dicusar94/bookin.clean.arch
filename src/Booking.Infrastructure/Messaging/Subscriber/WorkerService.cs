using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Booking.Infrastructure.Messaging.Subscriber;

public class WorkerService(ILogger<WorkerService> logger, RabbitMqReceiver rabbitMqReceiver) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Registering RabbitMQ listeners");
        rabbitMqReceiver.RegisterListeners();
        return Task.CompletedTask;
    }
    
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        if (rabbitMqReceiver is not null)
        {
            rabbitMqReceiver.Dispose();
        }
        
        return Task.CompletedTask;
    }
}