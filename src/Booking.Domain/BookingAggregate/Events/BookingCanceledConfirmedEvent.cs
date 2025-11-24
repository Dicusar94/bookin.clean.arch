using BookingApp.Entities;

namespace BookingApp.BookingAggregate.Events;

public record BookingCanceledConfirmedEvent(Guid Id) : IDomainEvent
{
    public string RoutingKey => GetType().FullName!;
}