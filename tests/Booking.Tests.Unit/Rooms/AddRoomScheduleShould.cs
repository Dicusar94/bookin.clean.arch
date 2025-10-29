using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class AddRoomScheduleShould
{
    [Fact]
    public void AddSchedule_should_add_schedule()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var schedule = TestRoomFactory.CreateRecurringSchedule(
            roomId: room.Id,
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        // act
        room.AddSchedule(schedule);
        
        // assert
        room.Schedules.Count.ShouldBe(1);
    }

    [Fact]
    public void AddSchedule_that_doesnt_overlap()
    {
        //arrange
        var room = TestRoomFactory.CreateRoom();

        var schedule1 = TestRoomFactory.CreateRecurringSchedule(
            roomId: room.Id,
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        var schedule2 = TestRoomFactory.CreateConcreteSchedule(
            roomId: room.Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.ElevenAmToTwelvePm);
        
        var schedule3 = TestRoomFactory.CreateConcreteSchedule(
            roomId: room.Id,
            date: DateTimeConstants.DateNow.AddDays(1),
            timeRange: TimeRangeConstants.ElevenAmToTwelvePm);
        
        room.AddSchedule(schedule1);
        room.AddSchedule(schedule2);
        
        // act
        room.AddSchedule(schedule3);
        
        // assert
        room.Schedules.Count.ShouldBe(3);
    }

    [Fact]
    public void AddSchedule_concrete_that_overlaps_with_recurring()
    {
        //arrange
        var room = TestRoomFactory.CreateRoom();

        var schedule1 = TestRoomFactory.CreateRecurringSchedule(
            roomId: room.Id,
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        var schedule2 = TestRoomFactory.CreateConcreteSchedule(
            roomId: room.Id,
            timeRange: TimeRangeConstants.TenAmToTwelvePm);
        
        room.AddSchedule(schedule1);
        
        // act
        room.AddSchedule(schedule2);
        
        // assert
        room.Schedules.Count.ShouldBe(2);
    }
    
    [Fact]
    public void AddSchedule_from_another_room_should_fail()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var schedule = TestRoomFactory.CreateRecurringSchedule(
            roomId: Guid.NewGuid(),
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        // act
        var action = () => room.AddSchedule(schedule);
        
        // assert
        action.ShouldThrow<Exception>();
    }

    [Fact]
    public void AddSchedule_that_overlaps_should_fail()
    {
        //arrange
        var room = TestRoomFactory.CreateRoom();

        var schedule1 = TestRoomFactory.CreateRecurringSchedule(
            roomId: room.Id,
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        var schedule2 = TestRoomFactory.CreateRecurringSchedule(
            roomId: room.Id,
            timeRange: TimeRangeConstants.TenAmToTwelvePm);
        
        room.AddSchedule(schedule1);
        
        // act
        var action = () => room.AddSchedule(schedule2);
        
        // assert
        action.ShouldThrow<Exception>();
    }
}