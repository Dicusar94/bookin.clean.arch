using BookingApp.Features.Rooms.RoomSchedules.Commands.AddRecurring;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.RoomSchedules;

[Collection(CollectionConstants.ApplicationTests)]
public class AddRecurringRoomScheduleCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory)
{
    private readonly ISender sender = apiFactory.GetService<ISender>();
    
    [Fact]
    public async Task Execute_should_succeed()
    {
        // arrange
        var command = new AddRecurringRoomScheduleCommand(
            Id: RoomConstants.Room2Id,
            DayOfWeek: DayOfWeek.Monday,
            Start: new TimeOnly(10, 0),
            End: new TimeOnly(12, 0));
        
        // act
        var schedule = await sender.Send(command);
        
        // assert
        schedule.ShouldNotBeNull();
    }
}