using BookingApp.RoomAggregate;
using BookingApp.Tests.Unit.Utils;
using BookingApp.Tests.Unit.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class RoomIsAvailableOnShould
{
    [Fact]
    public void IsAvailable_with_recurring_schedule_should_be_true()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var roomSchedule = RoomScheduleFactory.Recurring(
            roomId: room.Id,
            dayOfWeek: DateTimeConstants.DateNow.DayOfWeek,
            timeRange: TimeRangeConstants.EightAmToFivePm);
        
        room.AddSchedule(roomSchedule);

        // act
        var isAvailable = room.IsAvailableOn(
            date: DateTimeConstants.DateNow,
            requestedTime: TimeRangeConstants.FourAmToFivePm);
        
        // assert
        isAvailable.ShouldBeTrue();
    }
    
    [Fact]
    public void IsAvailable_with_specific_date_true()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var roomSchedule = RoomScheduleFactory.Concrete(
            roomId: room.Id,
            date: DateTimeConstants.DateNow,
            today: DateTimeConstants.DateTimeNow,
            timeRange: TimeRangeConstants.EightAmToFivePm);
        
        room.AddSchedule(roomSchedule);

        // act
        var isAvailable = room.IsAvailableOn(
            date: DateTimeConstants.DateNow,
            requestedTime: TimeRangeConstants.FourAmToFivePm);
        
        // assert
        isAvailable.ShouldBeTrue(); 
    }
    
    [Fact]
    public void IsAvailable_with_missing_specific_date_for_requested_date_false()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var roomSchedule = RoomScheduleFactory.Concrete(
            roomId: room.Id,
            date: DateTimeConstants.DateNow.AddDays(1),
            today: DateTimeConstants.DateTimeNow,
            timeRange: TimeRangeConstants.EightAmToFivePm);
        
        room.AddSchedule(roomSchedule);

        // act
        var isAvailable = room.IsAvailableOn(
            date: DateTimeConstants.DateNow,
            requestedTime: TimeRangeConstants.FourAmToFivePm);
        
        // assert
        isAvailable.ShouldBeFalse(); 
    }
    
    [Fact]
    public void IsAvailable_with_missing_recurring_day_for_requested_dayOfWeek_should_be_false()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var roomSchedule = RoomScheduleFactory.Recurring(
            roomId: room.Id,
            dayOfWeek: DateTimeConstants.DateNow.AddDays(1).DayOfWeek,
            timeRange: TimeRangeConstants.EightAmToFivePm);
        
        room.AddSchedule(roomSchedule);

        // act
        var isAvailable = room.IsAvailableOn(
            date: DateTimeConstants.DateNow,
            requestedTime: TimeRangeConstants.FourAmToFivePm);
        
        // assert
        isAvailable.ShouldBeFalse();
    }
}