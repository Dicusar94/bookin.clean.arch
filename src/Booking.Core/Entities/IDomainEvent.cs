using MediatR;

namespace BookingApp.Entities;

public interface IDomainEvent : INotification
{
    string RoutingKey { get; }
}