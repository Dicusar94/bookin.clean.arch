using BookingApp.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace BookingApp.Abstractions;

public abstract class BaseListener<T>(IServiceScopeFactory serviceScopeFactory) : IListener
{
    protected readonly IServiceProvider serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
    public virtual string RoutingKey => typeof(T).FullName!;
    public abstract Task ProcessMessage(Message message, string routingKey);
}