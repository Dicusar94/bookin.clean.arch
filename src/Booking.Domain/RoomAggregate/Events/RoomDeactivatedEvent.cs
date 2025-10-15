using BookingApp.Entities;

namespace BookingApp.RoomAggregate.Events;

public record RoomDeactivatedEvent(Guid Id, DateTime OnDateTime) : IDomainEvent
{
    public string RoutingKey => string.Empty;
}