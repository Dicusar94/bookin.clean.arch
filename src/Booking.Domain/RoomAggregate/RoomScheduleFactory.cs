using Booking.Domain.Commons;

namespace Booking.Domain.RoomAggregate;

public static class RoomScheduleFactory
{
    public static RoomSchedule Recurring(Guid roomId, DayOfWeek dayOfWeek, TimeRange timeRange)
    {
        return new RoomSchedule(
            roomId: roomId,
            dayOfWeek: dayOfWeek,
            isRecurring: true,
            timeRange: timeRange,
            date: null);
    }
    
    public static RoomSchedule Concrete(Guid roomId, TimeRange timeRange, DateOnly date, DateTime today)
    {
        if (new DateTime(date, timeRange.Start) < today.Date)
        {
            throw new Exception("Schedule start can't be in the past");
        }
        
        return new RoomSchedule(
            roomId: roomId,
            dayOfWeek: date.DayOfWeek,
            isRecurring: false,
            timeRange: timeRange,
            date: date);
    }
}