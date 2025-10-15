namespace BookingApp.Entities;

public interface IDomainEvent
{
    string RoutingKey { get; }
}