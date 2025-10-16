namespace BookingApp.RoomAggregate;

public interface IRoomRepository
{
    Task<Room> AddRoom(Room room);
    Task<Room> UpdateRoom(Room room);
    Task<Room> GetRoomById(Guid id);
}