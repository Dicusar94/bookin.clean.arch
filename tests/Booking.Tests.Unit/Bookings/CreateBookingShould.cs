using BookingApp.BookingAggregate.Events;
using BookingApp.Shared;
using BookingApp.Tests.Unit.Utils;
using BookingApp.Tests.Unit.Utils.TestContants.Bookings;
using BookingApp.Tests.Unit.Utils.TestContants.Rooms;
using BookingApp.Tests.Unit.Utils.TestContants.Schared;
using BookingApp.Tests.Unit.Utils.TestContants.Users;
using Shouldly;

namespace BookingApp.Tests.Unit.Bookings;

public class CreateBookingShould
{
    [Fact]
    public void Create_should_create()
    {
        // arrange & act
        var booking = TestBookingFactory.Create(
            roomId: RoomConstants.Id,
            userId: UserConstants.Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.NineAmToElevenAm,
            timeProvider: DateTimeConstants.TimeProvider,
            id: BookingConstants.Id);

        // assert
        booking.RoomId.ShouldBe(RoomConstants.Id);
        booking.UserId.ShouldBe(UserConstants.Id);
        booking.Date.ShouldBe(DateTimeConstants.DateNow);
        booking.TimeRange.ShouldBe(TimeRangeConstants.NineAmToElevenAm);
        booking.Id.ShouldBe(BookingConstants.Id);
    }
    
    [Fact]
    public void Create_should_raise_domain_events()
    {
        // arrange & act
        var booking = TestBookingFactory.Create(
            id: BookingConstants.Id,
            timeProvider: DateTimeConstants.TimeProvider);
        
        // assert
        booking.DomainEvents.Count.ShouldBe(2);
        
        var created = booking.DomainEvents.OfType<BookingCreatedEvent>().Single();
        created.Id.ShouldBe(booking.Id);

        var pendingConfirmation = booking.DomainEvents.OfType<BookingPendingConfirmationEvent>().Single();
        pendingConfirmation.Id.ShouldBe(booking.Id);
        pendingConfirmation.OnDate.ShouldBe(DateTimeConstants.TimeProvider.GetLocalNow());
    }

    [Fact]
    public void Create_with_datetime_in_past_should_fail()
    {
        // arrange & act
        var action = () => TestBookingFactory.Create(
            date: DateTimeConstants.DateNow.AddDays(-1),
            timeProvider: DateTimeConstants.TimeProvider);
        
        // assert
        action.ShouldThrow<Exception>();
    }

    [Fact]
    public void Create_with_invalid_timerange_should_fail()
    {
        // arrange & act
        var _15MinTimeRange = TimeRange.Create(
            start: new TimeOnly(0, 0),
            end: new TimeOnly(0, 15));
        
        // act 
        var action = () => TestBookingFactory.Create(
            date: DateTimeConstants.DateNow,
            timeRange: _15MinTimeRange);
        
        // assert
        action.ShouldThrow<Exception>();
    }
}