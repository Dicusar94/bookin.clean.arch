using BookingApp.BookingAggregate;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Bookings;

public class CancelBookingShould
{
    [Fact]
    public void Cancel_should_succeed()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var booking = TestBookingFactory.Create(timeProvider: timeProvider);
        booking.Confirm(timeProvider);
        
        // act
        booking.Cancel(timeProvider);
        
        // assert
        booking.Status.ShouldBe(BookingStatus.Canceled);
    }

    [Fact]
    public void Cancel_when_not_confirmed_status_should_fail()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var booking = TestBookingFactory.Create(timeProvider: timeProvider);
        
        // act
        var action = () => booking.Cancel(timeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }
}