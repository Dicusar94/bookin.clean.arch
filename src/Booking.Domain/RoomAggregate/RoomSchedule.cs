using Booking.Core.Entities;
using Booking.Domain.Commons;

namespace Booking.Domain.RoomAggregate;

public class RoomSchedule : Entity
{
    public Guid RoomId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public bool IsRecurring { get; set; }
    public TimeRange Time { get; set; } = null!;
    public DateOnly? OnDate { get; set; }
}