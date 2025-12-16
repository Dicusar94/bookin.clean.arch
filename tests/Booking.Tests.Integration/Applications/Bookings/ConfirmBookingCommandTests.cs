using BookingApp.BookingAggregate;
using BookingApp.Features.Bookings.Commands.Confirm;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Bookings;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Bookings;

[Collection(CollectionConstants.ApplicationTests)]
public class ConfirmBookingCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory)
{
    private readonly ApiFactory _apiFactory = apiFactory;

    [Fact]
    public async Task Execute_should_succeed()
    {
        //arrange
        var sender = _apiFactory.GetService<ISender>();
        var unitOfWork = _apiFactory.GetService<IUnitOfWork>();
        var command = new ConfirmBookingCommand(BookingConstants.Booking1Id);
        
        //act
        var response = await sender.Send(command);
        
        //assert
        var dbBooking = await unitOfWork.Bookings.GetBookingById(response.Id);
        dbBooking.Status.ShouldBe(BookingStatus.Confirmed);
    }
}