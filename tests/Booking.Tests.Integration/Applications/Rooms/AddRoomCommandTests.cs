using BookingApp.Features.Rooms.Rooms.Commands.Add;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Rooms;

[Collection(CollectionConstants.ApplicationTests)]
public class AddRoomCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory) 
{
    private readonly ISender sender = apiFactory.GetService<ISender>();
    
    [Fact]
    public async Task Execute_should_succeed()
    {
        //arrange
        var command = new AddRoomCommand(RoomConstants.Name, RoomConstants.Capacity);
        
        // act
        var room = await sender.Send(command);
        
        // assert
        room.Name.ShouldBe(RoomConstants.Name);
        room.Capacity.ShouldBe(RoomConstants.Capacity);
    }

    [Fact]
    public async Task Execute_when_room_with_same_name_exists_should_fail()
    {
        //arrange
        var command = new AddRoomCommand(RoomConstants.Name, RoomConstants.Capacity);
        _ = await sender.Send(command);

        // act
        var action = () => sender.Send(command);

        // assert
        action.ShouldThrow<Exception>();
    }
}
