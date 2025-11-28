using BookingApp.Messaging;

namespace BookingApp.Utils.Stubs;

public sealed class InMemorySyncMessageProducer(IEnumerable<IListener> listeners) : IMessageProducer
{
    public void PublishMessage(Message message, string routingKey)
    {
        foreach (var listener in listeners)
        {
            if (listener.RoutingKey == routingKey)
            {
                // Sync execution = perfect for tests
                listener.ProcessMessage(message, routingKey).GetAwaiter().GetResult();
            }
        }
    }
}