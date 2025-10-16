namespace BookingApp.RoomAggregate;

public interface IRoomRepository
{
    Task<Room> AddRoom(Room booking);
    Task<Room> UpdateRoom(Room booking);
    Task<Room> GetRoomById(Guid id);
}