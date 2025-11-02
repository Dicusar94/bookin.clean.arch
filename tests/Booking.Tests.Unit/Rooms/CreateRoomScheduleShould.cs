using BookingApp.RoomAggregate;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class CreateRoomScheduleShould
{
    [Fact]
    public void Recurring_should_create_a_recurring_room_schedule()
    {
        // arrange && act
        var schedule = RoomScheduleFactory.Recurring(
            roomId: RoomConstants.Room1Id,
            dayOfWeek: DateTimeConstants.DayOfWeek,
            timeRange: TimeRangeConstants.NineAmToElevenAm,
            id: RoomScheduleConstants.Room1Id1Schedule);
        
        // assert
        schedule.Id.ShouldBe(RoomScheduleConstants.Room1Id1Schedule);
        schedule.IsRecurring.ShouldBeTrue();
        schedule.DayOfWeek.ShouldBe(DateTimeConstants.DayOfWeek);
        schedule.TimeRange.ShouldBe(TimeRangeConstants.NineAmToElevenAm);
        schedule.Date.ShouldBeNull();
    }
    
    [Fact]
    public void Concrete_should_create_a_concrete_room_schedule()
    {
        // arrange && act
        var schedule = RoomScheduleFactory.Concrete(
            roomId: RoomConstants.Room1Id,
            timeRange: TimeRangeConstants.NineAmToElevenAm,
            date: DateTimeConstants.DateNow,
            today: DateTimeConstants.DateTimeNow,
            id: RoomScheduleConstants.Room1Id1Schedule);
        
        // assert
        schedule.Id.ShouldBe(RoomScheduleConstants.Room1Id1Schedule);
        schedule.IsRecurring.ShouldBeFalse();
        schedule.DayOfWeek.ShouldBe(DateTimeConstants.DateNow.DayOfWeek);
        schedule.TimeRange.ShouldBe(TimeRangeConstants.NineAmToElevenAm);
        schedule.Date.ShouldBe(DateTimeConstants.DateNow);
    }

    [Fact]
    public void Create_with_date_and_isrecurring_should_fail()
    {
        // arrange && act
        var action = () => new RoomSchedule(
            roomId: RoomConstants.Room1Id,
            dayOfWeek: DateTimeConstants.DayOfWeek,
            isRecurring: true,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Create_without_date_and_is_not_recurring_should_fail()
    {
        // arrange && act
        var action = () => new RoomSchedule(
            roomId: RoomConstants.Room1Id,
            dayOfWeek: DateTimeConstants.DayOfWeek,
            isRecurring: false,
            date: null,
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Concrete_with_date_in_the_past_should_fail()
    {
        // arrange && act
        var action = () => RoomScheduleFactory.Concrete(
            roomId: RoomConstants.Room1Id,
            timeRange: TimeRangeConstants.NineAmToElevenAm,
            date: DateTimeConstants.DateNow.AddDays(-1),
            today: DateTimeConstants.DateTimeNow,
            id: RoomScheduleConstants.Room1Id1Schedule);
        
        // assert
        action.ShouldThrow<Exception>();
    }
}