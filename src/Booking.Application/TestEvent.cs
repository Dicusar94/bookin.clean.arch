using Booking.Core.Entities;
using Booking.Core.Messaging;

namespace Booking.Application;

public class TestEvent : IDomainEvent
{
    public string RoutingKey => RoutingKeys.NameShout;
    public string Name { get; set; }
}