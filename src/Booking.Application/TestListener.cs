using System.Text.Json;
using BookingApp.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp;

public class TestListener(IServiceScopeFactory serviceScopeFactory) : IListener
{
    public string RoutingKey => RoutingKeys.NameShout;
    public async Task ProcessMessage(Message message, string routingKey)
    {
        await Task.Delay(TimeSpan.FromSeconds(5));
        var notification = JsonSerializer.Deserialize<TestEvent>(message.Body);

        if (notification is null)
        {
            Console.WriteLine("Empty message.. what to do ?");
        }
        
        Console.WriteLine($"Here is the message {notification!.Name}");
    }
}