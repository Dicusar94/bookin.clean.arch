using Booking.Core.Entities;

namespace Booking.Domain.RoomAggregate.Events;

public record RoomScheduleChangedEvent(Guid Id, Guid RoomScheduleId) : IDomainEvent
{
    public string RoutingKey => string.Empty;
}