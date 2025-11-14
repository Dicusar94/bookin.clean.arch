using BookingApp.Entities;

namespace BookingApp.BookingAggregate.Events;

public record BookingCanceledEvent(Guid Id) : IDomainEvent
{
    public string RoutingKey => GetType().FullName!;
}