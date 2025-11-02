using BookingApp.RoomAggregate;
using BookingApp.Shared;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;

namespace BookingApp.Utils;

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
            id: id ?? RoomConstants.Room1Id);
    }

    public static RoomSchedule CreateRecurringSchedule(
        Guid? roomId = null, 
        DayOfWeek? dayOfWeek = null,
        TimeRange? timeRange = null,
        Guid? id = null)
    {
        return RoomScheduleFactory.Recurring(
            roomId: roomId ?? RoomConstants.Room1Id,
            dayOfWeek: dayOfWeek ?? DayOfWeek.Monday,
            timeRange: timeRange ?? TimeRangeConstants.NineAmToElevenAm,
            id: id ?? Guid.NewGuid());
    }

    public static RoomSchedule CreateConcreteSchedule(
        Guid? roomId = null,
        TimeRange? timeRange = null,
        DateOnly? date = null,
        DateTime? today = null,
        Guid? id = null)
    {
        return RoomScheduleFactory.Concrete(
            roomId: roomId ?? RoomConstants.Room1Id,
            timeRange: timeRange ?? TimeRangeConstants.NineAmToElevenAm,
            date: date ?? DateTimeConstants.DateNow,
            today: today ?? DateTimeConstants.DateTimeNow,
            id: id ?? Guid.NewGuid());
    }
}