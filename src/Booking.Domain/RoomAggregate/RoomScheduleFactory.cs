using Booking.Domain.Commons;

namespace Booking.Domain.RoomAggregate;

public static class RoomScheduleFactory
{
    public static RoomSchedule Recurring(Guid roomId, DayOfWeek dayOfWeek, TimeRange timeRange, Guid? id = null)
    {
        return new RoomSchedule(
            roomId: roomId,
            dayOfWeek: dayOfWeek,
            isRecurring: true,
            timeRange: timeRange,
            date: null,
            id: id);
    }
    
    public static RoomSchedule Concrete(Guid roomId, TimeRange timeRange, DateOnly date, DateTime today, Guid? id = null)
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
            date: date,
            id: id);
    }
}