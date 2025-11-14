using BookingApp.Entities;
using BookingApp.Messaging;

namespace BookingApp;

public class TestEvent : IDomainEvent
{
    public string RoutingKey => GetType().FullName!;
    public string Name { get; set; }
}