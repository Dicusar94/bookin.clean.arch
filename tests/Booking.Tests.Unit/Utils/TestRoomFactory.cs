using Booking.Domain.RoomAggregate;
using Booking.Domain.Shared;
using Booking.Tests.Unit.Utils.TestContants;

namespace Booking.Tests.Unit.Utils;

public static class TestRoomFactory
{
    public static Room CreateRoom(
        string? name = null, 
        int? capacity = null, 
        Guid? id = null)
    {
        return new Room(
            name: name ?? RoomConstants.Name,
            capacity: capacity ?? RoomConstants.Capacity,
            id: id ?? RoomConstants.Id);
    }

    public static RoomSchedule CreateRecurringSchedule(
        Guid? roomId = null, 
        DayOfWeek? dayOfWeek = null,
        TimeRange? timeRange = null)
    {
        return RoomScheduleFactory.Recurring(
            roomId: roomId ?? RoomConstants.Id,
            dayOfWeek: dayOfWeek ?? DayOfWeek.Monday,
            timeRange: timeRange ?? TimeRangeConstants.NineToEleven);
    }

    public static RoomSchedule CreateConcreteSchedule(
        Guid? roomId = null,
        TimeRange? timeRange = null,
        DateOnly? date = null,
        DateTime? today = null)
    {
        return RoomScheduleFactory.Concrete(
            roomId: roomId ?? RoomConstants.Id,
            timeRange: timeRange ?? TimeRangeConstants.NineToEleven,
            date: date ?? DateTimeConstants.DateNow,
            today: today ?? DateTimeConstants.DateTimeNow);
    }
}