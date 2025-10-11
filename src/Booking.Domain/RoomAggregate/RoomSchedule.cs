using Booking.Core.Entities;
using Booking.Domain.Commons;

namespace Booking.Domain.RoomAggregate;

public class RoomSchedule : Entity
{
    public Guid RoomId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public bool IsRecurring { get; set; }
    public TimeRange TimeRange { get; set; } = null!;
    public DateOnly? Date { get; set; }
    
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
    
    private RoomSchedule(){}
}