using BookingApp.Features.Bookings.Backgrounds;
using BookingApp.Features.Bookings.Commands.Create;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using MediatR;
using Microsoft.Extensions.Time.Testing;
using Shouldly;

namespace BookingApp.Applications.Bookings;

[Collection(CollectionConstants.ApplicationTests)]
public class ScheduleAutoCancelBookingTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory), IClassFixture<ApiFactory>
{
    private readonly ApiFactory _apiFactory = apiFactory;

    [Fact]
    public async Task PendingBooking_ShouldSchedule_AutoCancelTicker()
    {
        //arrange
        var timeProvider = new FakeTimeProvider(DateTimeConstants.DateTimeOffsetNow);
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

        var expected = timeProvider.GetUtcNow()
            .DateTime.Add(BookingAggregate.Booking.MaxPendingStatusDuration);
        
        ticker.ExecutionTime.ShouldBe(expected);
    }
}