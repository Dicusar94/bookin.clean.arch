using BookingApp.Entities;

namespace BookingApp.RoomAggregate.Events;

public record RoomActivatedEvent(Guid Id, DateTime OnDateTime) : IDomainEvent
{
    public string RoutingKey => GetType().FullName!;
}