namespace BookingApp.RoomAggregate;

public interface IRoomRepository
{
    Task<Room> AddRoom(Room room, CancellationToken ct);
    Task<Room> UpdateRoom(Room room, CancellationToken ct);
    Task<Room> GetRoomById(Guid id, CancellationToken ct);
    Task<List<Room>> GetRooms(CancellationToken ct);
}