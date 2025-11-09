using BookingApp.Entities;
using BookingApp.Shared;

namespace BookingApp.RoomAggregate;

public class RoomSchedule : Entity
{
    public Guid RoomId { get; private set; }
    public DayOfWeek DayOfWeek { get; private set; }
    public bool IsRecurring { get; private set; }
    public TimeRange TimeRange { get; private set; } = null!;
    public DateOnly? Date { get; private set; }
    
    public override string ToString() => IsRecurring
        ? $"{DayOfWeek} ({TimeRange})"
        : $"{Date:yyyy-MM-dd} ({TimeRange})";
    
    internal RoomSchedule(
        Guid roomId,
        DayOfWeek dayOfWeek,
        bool isRecurring,
        TimeRange timeRange,
        DateOnly? date,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        if (isRecurring && date is not null)
        {
            throw new Exception("Recurring schedule can't be on a specific date");
        }

        if (!isRecurring && date is null)
        {
            throw new Exception("Date is require for non recurring schedule");
        }
        
        RoomId = roomId;
        IsRecurring = isRecurring;
        TimeRange = timeRange;
        DayOfWeek = dayOfWeek;
        Date = date;
    }

    public bool OverlapsWith(RoomSchedule other)
    {
        if (IsRecurring && other.IsRecurring)
        {
            return DayOfWeek == other.DayOfWeek && 
                   TimeRange.OverlapsWith(other.TimeRange);
        }

        if (!IsRecurring && !other.IsRecurring)
        {
            return Date == other.Date && 
                   TimeRange.OverlapsWith(other.TimeRange);
        }

        return false;
    }

    public bool CoversWith(DateOnly date, TimeRange requestedTime)
    {
        if (IsRecurring && date.DayOfWeek != DayOfWeek) return false;
        
        if (!IsRecurring && Date.HasValue && Date.Value != date) return false;

        return TimeRange.CoversWith(requestedTime);
    }

    public bool IsActive(TimeProvider timeProvider)
    {
        if (IsRecurring) return true;
        return timeProvider.GetUtcNow().DateTime < new DateTime(Date!.Value, TimeRange.End);
    }

    public RoomSchedule WithTimeRange(TimeRange newTimeRange)
    {
        return new RoomSchedule(
            roomId: RoomId,
            dayOfWeek: DayOfWeek,
            isRecurring: IsRecurring,
            timeRange: newTimeRange,
            date: Date);
    }

    public void RescheduleTimeRange(TimeRange newTimeRange)
    {
        TimeRange = newTimeRange;
    }
    
    private RoomSchedule(){}
}