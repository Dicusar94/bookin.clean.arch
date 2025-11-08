using BookingApp.Features.Rooms.RoomSchedules.Commands.Reschedule;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.RoomSchedules;

[Collection(CollectionConstants.ApplicationTests)]
public class RescheduleCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory)
{
    private readonly ISender sender = apiFactory.GetService<ISender>();
    
    [Fact]
    public async Task Execute_should_succeed()
    {
        // arrange
        var start = new TimeOnly(12, 0);
        var end = new TimeOnly(15, 0);
        
        var command = new RescheduleCommand(
            Id: RoomConstants.Room2Id,
            ScheduleId: RoomScheduleConstants.Room2Id1Schedule,
            Start: start,
            End: end);
        
        // act
        var schedule = await sender.Send(command);
        
        // assert
        schedule.TimeRange.Start.ShouldBe(start);
        schedule.TimeRange.End.ShouldBe(end);
    }
}