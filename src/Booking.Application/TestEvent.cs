using BookingApp.Entities;
using BookingApp.Messaging;

namespace BookingApp;

public class TestEvent : IDomainEvent
{
    public string RoutingKey => RoutingKeys.NameShout;
    public string Name { get; set; }
}