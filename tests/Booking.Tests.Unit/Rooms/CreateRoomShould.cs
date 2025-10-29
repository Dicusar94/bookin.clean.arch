using BookingApp.RoomAggregate;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class CreateRoomShould
{
    [Fact]
    public void Create_room()
    {
        // arrange & act
        var room = TestRoomFactory.CreateRoom(
            name: RoomConstants.Name,
            capacity: RoomConstants.Capacity,
            id: RoomConstants.Id);
        
        // assert
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
        // arrange & act
        var action = () => TestRoomFactory.CreateRoom(
            name: name,
            capacity: capacity,
            id: RoomConstants.Id);
        
        // assert
        action.ShouldThrow<Exception>();
    }
}