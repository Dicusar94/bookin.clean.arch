using BookingApp.Entities;

namespace BookingApp.BookingAggregate.Events;

public record BookingPendingConfirmationEvent(Guid Id, DateTimeOffset OnDate) : IDomainEvent
{
    public string RoutingKey => string.Empty;
}