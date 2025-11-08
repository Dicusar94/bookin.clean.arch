namespace BookingApp.Features.Rooms.RoomSchedules.Commons;

public class RoomScheduleDto
{
    public Guid RoomId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public bool IsRecurring { get; set; }
    public TimeRangeDto TimeRange { get; set; } = null!;
    public DateOnly? Date { get; set; }
}