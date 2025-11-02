using BookingApp.Features.Rooms.Rooms.Commands.Add;
using BookingApp.Features.Rooms.Rooms.Commands.Deactivate;
using BookingApp.RoomAggregate;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Rooms;

[Collection(CollectionConstants.ApplicationTests)]
public class DeactivateRoomCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory) 
{
    private readonly ISender sender = apiFactory.GetService<ISender>();

    [Fact]
    public async Task Execute_should_succeed()
    {
        //arrange
        var command = new DeactivateRoomCommand(RoomConstants.Room1Id);
        
        // act
        var room = await sender.Send(command);
        
        // assert
        room.Status.ShouldBe(RoomStatus.Inactive);
    }

    public Task InitializeAsync()
    {
        return _initDatabase();
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
        await _seedDatabase();
    }
}
