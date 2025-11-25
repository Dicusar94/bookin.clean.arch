using BookingApp.RoomAggregate;
using BookingApp.RoomAggregate.Events;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using Microsoft.Extensions.Time.Testing;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class ActivateRoomShould
{
    [Fact]
    public void Activate_should_activate()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        room.AddSchedule(TestRoomFactory.CreateRecurringSchedule());
        
        // act
        room.Activate(DateTimeConstants.FakeProvider);
        
        // assert
        room.Status.ShouldBe(RoomStatus.Active);
    }
    
    [Fact]
    public void Activate_when_room_is_active_should_fail()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        room.AddSchedule(TestRoomFactory.CreateRecurringSchedule());
        room.Activate(DateTimeConstants.FakeProvider);
        
        // act
        var action = () => room.Activate(DateTimeConstants.FakeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }

    [Fact]
    public void Activate_when_has_no_schedule_should_fail()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        // act
        var action = () => room.Activate(DateTimeConstants.FakeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Activate_when_has_no_active_schedule_should_fail()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var room = TestRoomFactory.CreateRoom();
        room.AddSchedule(TestRoomFactory.CreateConcreteSchedule(date: DateTimeConstants.DateNow));
        
        timeProvider.Advance(TimeSpan.FromDays(1));
        
        // act
        var action = () => room.Activate(timeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Deactivate_should_raise_domain_event()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        room.AddSchedule(TestRoomFactory.CreateRecurringSchedule());
        
        // act
        room.Activate(DateTimeConstants.FakeProvider);
        
        // assert
        room.DomainEvents.OfType<RoomActivatedEvent>().Count().ShouldBe(1);

        var roomDeactivatedEvent = room.DomainEvents.OfType<RoomActivatedEvent>().Single();
        roomDeactivatedEvent.Id.ShouldBe(room.Id);
        roomDeactivatedEvent.OnDateTime.ShouldBe(DateTimeConstants.FakeProvider.GetUtcNow().UtcDateTime);
    }
}