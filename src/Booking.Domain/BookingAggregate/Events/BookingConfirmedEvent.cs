using BookingApp.Entities;

namespace BookingApp.BookingAggregate.Events;

public record BookingConfirmedEvent(Guid Id) : IDomainEvent
{
    public string RoutingKey => GetType().FullName!;
}