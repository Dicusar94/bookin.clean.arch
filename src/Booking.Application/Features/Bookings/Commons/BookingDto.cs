using BookingApp.Features.Rooms.RoomSchedules.Commons;

namespace BookingApp.Features.Bookings.Commons;

public class BookingDto
{
    public Guid RoomId { get; set; }
    public Guid UserId { get; set; }
    public DateOnly Date { get; set; }
    public TimeRangeDto TimeRange { get; set; } = null!;
    public BookingStatusDto Status { get; set; }
}