using Booking.Domain.RoomAggregate;
using Booking.Tests.Unit.Utils.TestContants;
using Shouldly;

namespace Booking.Tests.Unit.Rooms;

public class CreateRoomScheduleShould
{
    [Fact]
    public void Recurring_should_create_a_recurring_room_schedule()
    {
        // arrange && act
        var schedule = RoomScheduleFactory.Recurring(
            roomId: RoomConstants.Id,
            dayOfWeek: DateTimeConstants.DayOfWeek,
            timeRange: TimeRangeConstants.NineToEleven,
            id: RoomScheduleConstants.Id1);
        
        // assert
        schedule.Id.ShouldBe(RoomScheduleConstants.Id1);
        schedule.IsRecurring.ShouldBeTrue();
        schedule.DayOfWeek.ShouldBe(DateTimeConstants.DayOfWeek);
        schedule.TimeRange.ShouldBe(TimeRangeConstants.NineToEleven);
        schedule.Date.ShouldBeNull();
    }
    
    [Fact]
    public void Concrete_should_create_a_concrete_room_schedule()
    {
        // arrange && act
        var schedule = RoomScheduleFactory.Concrete(
            roomId: RoomConstants.Id,
            timeRange: TimeRangeConstants.NineToEleven,
            date: DateTimeConstants.DateNow,
            today: DateTimeConstants.DateTimeNow,
            id: RoomScheduleConstants.Id1);
        
        // assert
        schedule.Id.ShouldBe(RoomScheduleConstants.Id1);
        schedule.IsRecurring.ShouldBeFalse();
        schedule.DayOfWeek.ShouldBe(DateTimeConstants.DateNow.DayOfWeek);
        schedule.TimeRange.ShouldBe(TimeRangeConstants.NineToEleven);
        schedule.Date.ShouldBe(DateTimeConstants.DateNow);
    }
}