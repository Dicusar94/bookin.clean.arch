using BookingApp.BookingAggregate;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Bookings;

public class ConfirmBookingShould
{
    [Fact]
    public void Confirm_should_succeed()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var booking = TestBookingFactory.Create(timeProvider: timeProvider);
        
        // act
        booking.Confirm(timeProvider);
        
        // assert
        booking.Status.ShouldBe(BookingStatus.Confirmed);
    }
    
    [Fact]
    public void Confirm_with_not_pending_status_should_succeed()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var booking = TestBookingFactory.Create(timeProvider: timeProvider);
        
        timeProvider.Advance(Booking.MaxPendingStatusDuration.Add(TimeSpan.FromMinutes(1)));
        booking.AutoCancel(timeProvider);
        
        // act
        var action = () => booking.Confirm(timeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }
    
    
    [Fact]
    public void Confirm_with_expired_pending_status_max_duration_should_succeed()
    {
        // arrange
        var timeProvider = DateTimeConstants.FakeProvider;
        var booking = TestBookingFactory.Create(timeProvider: timeProvider);
        
        timeProvider.Advance(Booking.MaxPendingStatusDuration.Add(TimeSpan.FromMinutes(1)));
        
        // act
        var action = () => booking.Confirm(timeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }
}