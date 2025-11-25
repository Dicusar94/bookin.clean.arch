using BookingApp.RoomAggregate;
using BookingApp.RoomAggregate.Events;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
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
        room.AddSchedule(TestRoomFactory.CreateRecurringSchedule());
        room.Activate(DateTimeConstants.FakeProvider);
        
        // act
        room.Deactivate(TimeProvider.System);
        
        // assert
        room.Status.ShouldBe(RoomStatus.Inactive);
    }
    
    [Fact]
    public void Deactivate_when_room_is_inactive_should_fail()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        // act
        var action = () => room.Deactivate(DateTimeConstants.FakeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Deactivate_should_raise_domain_event()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        room.AddSchedule(TestRoomFactory.CreateRecurringSchedule());
        room.Activate(DateTimeConstants.FakeProvider);
        
        // act
        room.Deactivate(DateTimeConstants.FakeProvider);
        
        // assert
        room.DomainEvents.OfType<RoomDeactivatedEvent>().Count().ShouldBe(1);

        var roomDeactivatedEvent = room.DomainEvents.OfType<RoomDeactivatedEvent>().Single();
        roomDeactivatedEvent.Id.ShouldBe(room.Id);
        roomDeactivatedEvent.OnDateTime.ShouldBe(DateTimeConstants.FakeProvider.GetUtcNow().UtcDateTime);
    }
}