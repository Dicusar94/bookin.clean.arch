using BookingApp.Exceptions;
using BookingApp.Features.Rooms.Rooms.Queries.GetById;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Rooms;

[Collection(CollectionConstants.ApplicationTests)]
public class GetRoomByIdQueryTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory) 
{
    private readonly ISender sender = apiFactory.GetService<ISender>();

    [Fact]
    public async Task Execute_should_succeed()
    {
        // arrange
        var query = new GetRoomByIdQuery(RoomConstants.Room1Id);
        
        // act
        var room = await sender.Send(query);
        
        // assert
        room.Id.ShouldBe(RoomConstants.Room1Id);
    }
    
    [Fact]
    public void Execute_when_no_room_should_fail()
    {
        // arrange
        var query = new GetRoomByIdQuery(RoomConstants.Room3Id);
        
        // act
        var action = () => sender.Send(query);
        
        // assert
        action.ShouldThrow<EntityNotFoundException>();
    }
}