using BookingApp.RoomAggregate;
using BookingApp.RoomAggregate.Events;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class ActivateRoomShould
{
    [Fact]
    public void Activate_should_activate()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        // act
        room.Activate(DateTimeConstants.TimeProvider);
        
        // assert
        room.Status.ShouldBe(RoomStatus.Active);
    }
    
    [Fact]
    public void Activate_when_room_is_active_should_fail()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        room.Activate(DateTimeConstants.TimeProvider);
        
        // act
        var action = () => room.Activate(TimeProvider.System);
        
        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Deactivate_should_raise_domain_event()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        // act
        room.Activate(DateTimeConstants.TimeProvider);
        
        // assert
        room.DomainEvents.OfType<RoomActivatedEvent>().Count().ShouldBe(1);

        var roomDeactivatedEvent = room.DomainEvents.OfType<RoomActivatedEvent>().Single();
        roomDeactivatedEvent.Id.ShouldBe(room.Id);
        roomDeactivatedEvent.OnDateTime.ShouldBe(DateTimeConstants.TimeProvider.GetUtcNow().DateTime);
    }
}