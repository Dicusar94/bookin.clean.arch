using BookingApp.Features.Bookings.BackgroundJobs.AutoCancels;
using BookingApp.Features.Bookings.Commands.Create;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Bookings;

public class ScheduleAutoCancelBookingShould(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory), IClassFixture<ApiFactory>
{
    private readonly ApiFactory _apiFactory = apiFactory;

    [Fact]
    public async Task PendingBooking_ShouldSchedule_AutoCancelTicker()
    {
        //arrange
        var fakeTicker = _apiFactory.GetTickerService<BookingAutoCancelTicker>();
        var sender = _apiFactory.GetService<ISender>();

        //act
        var timeRange = TimeRangeConstants.NineAmToElevenAm;
        
        var command = new CreateBookingCommand(
            RoomId: RoomConstants.Room1Id,
            UserId: UserConstants.User3Id,
            Date: DateTimeConstants.DateNow,
            Start: timeRange.Start,
            End: timeRange.End);

        var response = await sender.Send(command);
        
        //assert
        fakeTicker.AddedTickers.Count.ShouldBe(1);
        var ticker = fakeTicker.AddedTickers.First();

        var expected = DateTimeConstants.TimeProvider.GetUtcNow()
            .DateTime.Add(BookingAggregate.Booking.MaxPendingStatusDuration);
        
        ticker.ExecutionTime.ShouldBe(expected);
    }
}