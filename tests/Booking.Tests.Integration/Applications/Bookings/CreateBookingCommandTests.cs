using BookingApp.Features.Bookings.Commands.Create;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using MediatR;
using Shouldly;

namespace BookingApp.Applications.Bookings;


[Collection(CollectionConstants.ApplicationTests)]
public class CreateBookingCommandTests(ApiFactory apiFactory) : BaseAsyncLifeTime(apiFactory)
{
    private readonly ISender _sender = apiFactory.GetService<ISender>();

    [Fact]
    public async Task Execute_should_succeed()
    {
        // arrange
        var command = new CreateBookingCommand(
            RoomId: RoomConstants.Room2Id,
            UserId: UserConstants.User3Id,
            Date: DateTimeConstants.DateNow,
            Start: new TimeOnly(10, 00),
            End: new TimeOnly(11, 00));

        // act
        var booking = await _sender.Send(command, CancellationToken.None);
        
        // assert
        booking.ShouldNotBeNull();
    }
}