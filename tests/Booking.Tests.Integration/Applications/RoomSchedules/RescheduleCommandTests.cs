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
        var timeRange = TimeRange.Create(start, end);
        
        var command = new RescheduleCommand(
            Id: RoomConstants.Room2Id,
            ScheduleId: RoomScheduleConstants.Room2Id1Schedule,
            Start: timeRange.Start,
            End: timeRange.End);
        
        // act
        var schedule = await sender.Send(command);
        
        // assert
        schedule.TimeRange.ShouldBe(timeRange);
    }
}