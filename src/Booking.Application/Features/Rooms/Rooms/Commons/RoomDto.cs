using BookingApp.Features.Rooms.RoomSchedules.Commons;

namespace BookingApp.Features.Rooms.Rooms.Commons;

public class RoomDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public RoomStatusDto Status { get; set; }
    public IReadOnlyList<RoomScheduleDto> Schedules { get; set; } = [];
}