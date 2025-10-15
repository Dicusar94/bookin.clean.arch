using BookingApp.Entities;

namespace BookingApp.RoomAggregate.Events;

public record RoomScheduleChangedEvent(Guid Id, Guid RoomScheduleId) : IDomainEvent
{
    public string RoutingKey => string.Empty;
}