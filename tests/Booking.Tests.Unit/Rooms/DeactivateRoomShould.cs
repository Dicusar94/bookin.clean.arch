using BookingApp.RoomAggregate;
using BookingApp.RoomAggregate.Events;
using BookingApp.Utils;
using Microsoft.Extensions.Time.Testing;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class DeactivateRoomShould
{
    [Fact]
    public void Deactivate_should_deactive()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        // act
        room.Deactivate(TimeProvider.System);
        
        // assert
        room.Status.ShouldBe(RoomStatus.Inactive);
    }
    
    [Fact]
    public void Deactivate_should_raise_domain_event()
    {
        // arrange
        var dateTime = DateTimeOffset.UtcNow;
        var timeProvider = new FakeTimeProvider(dateTime);
        var room = TestRoomFactory.CreateRoom();
        
        // act
        room.Deactivate(timeProvider);
        
        // assert
        room.DomainEvents.OfType<RoomDeactivatedEvent>().Count().ShouldBe(1);

        var roomDeactivatedEvent = room.DomainEvents.OfType<RoomDeactivatedEvent>().Single();
        roomDeactivatedEvent.Id.ShouldBe(room.Id);
        roomDeactivatedEvent.OnDateTime.ShouldBe(dateTime.DateTime);
    }
}