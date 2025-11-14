using BookingApp.Entities;

namespace BookingApp.BookingAggregate.Events;

public record BookingAutoCanceledEvent(Guid Id) : IDomainEvent
{
    public string RoutingKey => GetType().FullName!;
}