using BookingApp.BookingAggregate;
using BookingApp.Features.Bookings.Commands.Cancel;
using BookingApp.Features.Bookings.Commands.Confirm;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Bookings;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Bookings;

[Collection(CollectionConstants.ApplicationTests)]
public class CancelBookingCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory)
{
    private readonly ApiFactory _apiFactory = apiFactory;

    [Fact]
    public async Task Execute_should_succeed()
    {
        // arrange
        var sender = _apiFactory.GetService<ISender>();
        var unitOfWork = _apiFactory.GetService<IUnitOfWork>();
        
        var bookingId = BookingConstants.Booking1Id;
        var confirmCommand = new ConfirmBookingCommand(bookingId);
        await sender.Send(confirmCommand);

        var command = new CancelBookingCommand(bookingId);

        // act
        _ = await sender.Send(command);

        // assert
        var dbBooking = await unitOfWork.Bookings.GetBookingById(bookingId);
        dbBooking.Status.ShouldBe(BookingStatus.Canceled);
    }
}