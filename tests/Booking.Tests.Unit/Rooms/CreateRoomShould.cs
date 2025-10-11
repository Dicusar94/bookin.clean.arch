using Booking.Domain.RoomAggregate;
using Booking.Tests.Unit.Utils;
using Booking.Tests.Unit.Utils.TestContants;
using Shouldly;

namespace Booking.Tests.Unit.Rooms;

public class CreateRoomShould
{
    [Fact]
    public void Create_room()
    {
        // Arrange & Act
        var room = TestRoomFactory.CreateRoom(
            name: RoomConstants.Name,
            capacity: RoomConstants.Capacity,
            id: RoomConstants.Id);
        
        // Assert
        room.Id.ShouldBe(RoomConstants.Id);
        room.Name.ShouldBe(RoomConstants.Name);
        room.Capacity.ShouldBe(RoomConstants.Capacity);
        room.Status.ShouldBe(RoomStatus.Inactive);
    }
    
    [Theory]
    [InlineData(" ", 1)]
    [InlineData("room 1", 0)]
    [InlineData("room 1", -1)]
    public void Create_fail(string name, int capacity)
    {
        // Arrange & Act
        var action = () => TestRoomFactory.CreateRoom(
            name: name,
            capacity: capacity,
            id: RoomConstants.Id);
        
        // Assert
        action.ShouldThrow<Exception>();
    }
}