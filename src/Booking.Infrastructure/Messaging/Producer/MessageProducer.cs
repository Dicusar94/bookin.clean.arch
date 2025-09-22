using System.Text;
using Booking.Core.Messaging;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Booking.Infrastructure.Messaging.Producer;

public class MessageProducer : IMessageProducer
{
    private readonly IModel _channel;
    private readonly RabbitMQSettings _rabbitSettings;
    private readonly ILogger<MessageProducer> _logger;

    public MessageProducer(
        IModel channel,
        RabbitMQSettings rabbitMqSettings,
        ILogger<MessageProducer> logger)
    {
        _channel = channel;
        _rabbitSettings = rabbitMqSettings;
        _logger = logger;
    }
    
    
    public void PublishMessage(Message message, string routingKey)
    {
        // add open telemetry traceparent id

        var properties = _channel.CreateBasicProperties();
        properties.ContentType = "text/plain";
        properties.Headers = message.Header.Properties;

        var body = Encoding.UTF8.GetBytes(message.Body);

        try
        {
            _channel.BasicPublish(
                exchange: _rabbitSettings.ExchangeName,
                routingKey: routingKey,
                basicProperties: properties,
                body: body);
            
            
            _logger.LogInformation("Published message with key {Key} {Message}", routingKey, message);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed publishing message");
            throw;
        }
    }
}