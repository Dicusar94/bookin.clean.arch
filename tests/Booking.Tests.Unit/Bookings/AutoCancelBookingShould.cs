using BookingApp.BookingAggregate;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Bookings;

public class AutoCancelBookingShould
{
    [Fact]
    public void AutoCancel_should_succeed()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var booking = TestBookingFactory.Create(timeProvider: timeProvider);

        var timeAdvance = Booking.MaxPendingStatusDuration;
        timeProvider.Advance(timeAdvance);
        
        // act
        booking.AutoCancel(timeProvider);
        
        // assert
        booking.Status.ShouldBe(BookingStatus.Canceled);
    }
    
    [Fact]
    public void AutoCancel_with_confirmed_status_should_failed()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var booking = TestBookingFactory.Create(timeProvider: timeProvider);
        booking.Confirm(timeProvider);
        
        // act
        var action = () => booking.AutoCancel(timeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }
}