using Booking.Domain.Commons;
using Booking.Domain.RoomAggregate;
using Booking.Tests.Unit.Utils.TestContants;

namespace Booking.Tests.Unit.Utils;

public static class TestRoomFactory
{
    public static Room CreateRoom(string? name, int? capacity, Guid? id)
    {
        return new Room(
            name: name ?? RoomConstants.Name,
            capacity: capacity ?? RoomConstants.Capacity,
            id: id ?? RoomConstants.Id);
    }

    public static RoomSchedule CreateRecurringSchedule(
        Guid? roomId, 
        DayOfWeek? dayOfWeek,
        TimeRange? timeRange)
    {
        return RoomScheduleFactory.Recurring(
            roomId: roomId ?? RoomConstants.Id,
            dayOfWeek: dayOfWeek ?? DayOfWeek.Monday,
            timeRange: timeRange ?? TimeRangeConstants.NineToEleven);
    }

    public static RoomSchedule CreateConcreteSchedule(
        Guid? roomId,
        TimeRange? timeRange,
        DateOnly? date,
        DateTime? today)
    {
        return RoomScheduleFactory.Concrete(
            roomId: roomId ?? RoomConstants.Id,
            timeRange: timeRange ?? TimeRangeConstants.NineToEleven,
            date: date ?? DateTimeConstants.DateNow,
            today: today ?? DateTimeConstants.DateTimeNow);
    }
}