using BookingApp.Features.Bookings.Commands.Confirm;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Bookings;
using MediatR;

namespace BookingApp.Applications.Bookings;

[Collection(CollectionConstants.ApplicationTests)]
public class ConfirmBookingCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory), IClassFixture<ApiFactory>
{
    private readonly ApiFactory _apiFactory = apiFactory;

    public async Task Execute_should_succeed()
    {
        //arrange
        var sender = _apiFactory.GetService<ISender>();
        var command = new ConfirmBookingCommand(BookingConstants.Booking1Id);
        
        //act
        var response = await sender.Send(command);
        
        //assert
        return;
    }
}