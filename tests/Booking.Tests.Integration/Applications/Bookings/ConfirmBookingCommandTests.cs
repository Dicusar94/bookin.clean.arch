using BookingApp.Utils;

namespace BookingApp.Applications.Bookings;

[Collection(CollectionConstants.ApplicationTests)]
public class ConfirmBookingCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory), IClassFixture<ApiFactory>
{
    public Task Execute_should_succeed()
    {
        //arrange
        object? command = null;
        
        //act
        
        //assert
        return Task.CompletedTask;
    }
}