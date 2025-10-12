using Booking.Core.Entities;

namespace Booking.Domain.RoomAggregate.Events;

public record RoomDeactivatedEvent(Guid Id, DateTime OnDateTime) : IDomainEvent
{
    public string RoutingKey => string.Empty;
}