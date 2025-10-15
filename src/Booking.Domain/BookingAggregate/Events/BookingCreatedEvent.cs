using BookingApp.Entities;

namespace BookingApp.BookingAggregate.Events;

public record BookingCreatedEvent(Guid Id) : IDomainEvent
{
    public string RoutingKey => string.Empty;
}