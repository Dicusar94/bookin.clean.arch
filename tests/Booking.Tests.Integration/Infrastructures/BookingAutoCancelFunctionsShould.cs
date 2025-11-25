using BookingApp.BookingAggregate;
using BookingApp.Features.Bookings.BackgroundJobs.AutoCancels;
using BookingApp.Features.Bookings.Commands.Create;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.Stubs;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using MediatR;
using Org.BouncyCastle.Asn1.X509;
using Shouldly;

namespace BookingApp.Infrastructures;

public class BookingAutoCancelFunctionsShould(ApiFactory apiFactory) 
    : BaseAsyncLifeTime(apiFactory), IClassFixture<ApiFactory>
{
    private readonly ApiFactory _apiFactory = apiFactory;

    [Fact]
    public async Task AutoCancel_ShouldCancelBooking()
    {
        var sender = _apiFactory.GetService<ISender>();
        var unitOfWork = _apiFactory.GetService<IUnitOfWork>();
        var timeRange = TimeRangeConstants.NineAmToElevenAm;
        
        var command = new CreateBookingCommand(
            RoomId: RoomConstants.Room1Id,
            UserId: UserConstants.User3Id,
            Date: DateTimeConstants.DateNow,
            Start: timeRange.Start,
            End: timeRange.End);

        var response = await sender.Send(command);
        
        var timeTravelSpan = BookingAggregate.Booking.MaxPendingStatusDuration + TimeSpan.FromMinutes(1);
        TimeProvider.Advance(timeTravelSpan);
        var function = _apiFactory.GetService<BookingAutoCancelFunctions>();
        
        // act
        var context = TickerFunctionFactory.Create(response.Id);
        await function.Execute(context, CancellationToken.None);

        // assert
        var dbBooking = await unitOfWork.Bookings.GetBookingById(response.Id);
        dbBooking.Status.ShouldBe(BookingStatus.Canceled);
    }
}