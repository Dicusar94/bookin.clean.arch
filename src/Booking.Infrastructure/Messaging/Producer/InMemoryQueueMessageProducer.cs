using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Messaging.Producer;

public class InMemoryQueueMessageProducer : IMessageProducer
{
    private readonly Channel<(Message Message, string RoutingKey)> _queue 
        = Channel.CreateUnbounded<(Message, string)>();
    
    public InMemoryQueueMessageProducer(IServiceProvider serviceProvider)
    {
        
        Task.Run(async () =>
        {
            await foreach (var (message, routingKey) in _queue.Reader.ReadAllAsync())
            {
                using var scope = serviceProvider.CreateScope();
                var eventHandlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IListener>>();
                foreach (var listener in eventHandlers)
                {
                    if (listener.RoutingKey == routingKey)
                    {
                        await listener.ProcessMessage(message, routingKey);
                    }
                }
            }
        });
    }

    public void PublishMessage(Message message, string routingKey)
    {
        _queue.Writer.TryWrite((message, routingKey));
    }
}