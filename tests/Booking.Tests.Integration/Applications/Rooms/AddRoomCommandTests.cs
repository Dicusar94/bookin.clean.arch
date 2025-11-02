using BookingApp.Features.Rooms.Rooms.Commands.Add;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Rooms;

[Collection(CollectionConstants.ApplicationTests)]
public class AddRoomCommandTests(ApiFactory apiFactory) : IAsyncLifetime
{
    private readonly ISender sender = apiFactory.GetService<ISender>();
    
    private readonly Func<Task> _initDatabase = apiFactory.TestDatabaseReset.InitializeAsync;
    private readonly Func<Task> _resetDatabase = apiFactory.TestDatabaseReset.ResetAsync;
    private readonly Func<Task> _seedDatabase = apiFactory.SeedAsync;
    
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
