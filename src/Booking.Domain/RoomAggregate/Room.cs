using Booking.Core.Entities;

namespace Booking.Domain.RoomAggregate;

public class Room : AggregateRoot
{
    private readonly List<RoomSchedule> _schedules = [];
    
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public RoomStatus Status { get; set; }
    public IReadOnlyList<RoomSchedule> Schedules => _schedules.AsReadOnly();
}