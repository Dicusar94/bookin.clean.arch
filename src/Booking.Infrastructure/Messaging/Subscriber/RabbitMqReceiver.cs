using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BookingApp.Messaging.Subscriber;

public class RabbitMqReceiver
{
    private readonly RabbitMQSettings _rabbitMqSettings;
    private readonly IModel? _channel;
    private readonly List<IListener> _listeningServices;

    public RabbitMqReceiver(IServiceProvider sp, RabbitMQSettings rabbitMqSettings, IModel channel)
    {
        _listeningServices = sp.GetServices<IListener>().ToList();
        _rabbitMqSettings = rabbitMqSettings;
        _channel = channel;
    }

    public void RegisterListeners()
    {
        _listeningServices.ForEach(Listener);
    }

    public void Dispose()
    {
        if (_channel == null)
        {
            Console.WriteLine("Warning: Channel was already disposed or not initialized");
            return;
        }
        
        _channel.Dispose();
    }

    private void Listener(IListener services)
    {
        _channel.ExchangeDeclare(
            exchange: _rabbitMqSettings.ExchangeName,
            type: _rabbitMqSettings.ExchangeType);

        var queueName = _channel.QueueDeclare(
            queue: _rabbitMqSettings.AppPrefix + Guid.NewGuid()).QueueName;
        
        _channel.QueueBind(
            queue: queueName,
            exchange: _rabbitMqSettings.ExchangeName,
            routingKey: services.RoutingKey);

        var consumerAsync = new AsyncEventingBasicConsumer(_channel);

        consumerAsync.Received += async (_, ea) =>
        {
            var parentContext = PropagateContextFromRabbitHeaders(ea.BasicProperties);
            //use otel to set activity with parent context traceparent

            var body = ea.Body.ToArray();

            var message = new Message(
                header: new Header { Properties = ea.BasicProperties.Headers },
                serializedObject: Encoding.UTF8.GetString(body));

            await services.ProcessMessage(message, ea.RoutingKey);
            _channel?.BasicAck(ea.DeliveryTag, false);
        };

        _channel.BasicConsume(
            queue: queueName,
            autoAck: false,
            consumer: consumerAsync);
    }
    
    private static ActivityContext PropagateContextFromRabbitHeaders(IBasicProperties props)
    {
        if (props.Headers != null && props.Headers.TryGetValue("traceparent", out var traceParentObj))
        {
            var traceParent = Encoding.UTF8.GetString((byte[]) traceParentObj);
            var ctx = ActivityContext.Parse(traceParent, null);
            return ctx;
        }

        return default;
    }
}