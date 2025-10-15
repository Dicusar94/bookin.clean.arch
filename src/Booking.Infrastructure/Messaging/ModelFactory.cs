using RabbitMQ.Client;

namespace BookingApp.Messaging;

public class ModelFactory(IConnectionFactory connectionFactory, RabbitMQSettings rabbitMqSettings) : IDisposable
{
    private readonly IConnection _connection  = connectionFactory.CreateConnection();

    public IModel CreateChannel()
    {
        var channel = _connection.CreateModel();
        
        channel.ExchangeDeclare(
            exchange: rabbitMqSettings.ExchangeName,
            type: rabbitMqSettings.ExchangeType);

        return channel;
    }
    
    public void Dispose()
    {
        _connection.Dispose();
    }
}