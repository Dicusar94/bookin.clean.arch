using Booking.Domain.RoomAggregate;
using Booking.Tests.Unit.Utils.TestContants;

namespace Booking.Tests.Unit.Utils;

public static class RoomFactory
{
    public static Room CreateRoom(string? name, int? capacity, Guid? id)
    {
        return new Room(
            name: name ?? RoomConstants.Name,
            capacity: capacity ?? RoomConstants.Capacity,
            id: id ?? RoomConstants.Id);
    }
}