using BookingApp.BookingAggregate.Events;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Bookings;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using Shouldly;

namespace BookingApp.Tests.Unit.Bookings;

public class CreateBookingShould
{
    [Fact]
    public void Create_should_create()
    {
        // arrange & act
        var booking = TestBookingFactory.Create(
            roomId: RoomConstants.Room1Id,
            userId: UserConstants.User1Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.NineAmToElevenAm,
            timeProvider: DateTimeConstants.FakeProvider,
            id: BookingConstants.Booking1Id);

        // assert
        booking.RoomId.ShouldBe(RoomConstants.Room1Id);
        booking.UserId.ShouldBe(UserConstants.User1Id);
        booking.Date.ShouldBe(DateTimeConstants.DateNow);
        booking.TimeRange.ShouldBe(TimeRangeConstants.NineAmToElevenAm);
        booking.Id.ShouldBe(BookingConstants.Booking1Id);
        booking.CreatedAt.ShouldBe(DateTimeConstants.DateTimeNow);
        booking.ConfirmedAt.ShouldBeNull();
        booking.CanceledAt.ShouldBeNull();
    }
    
    [Fact]
    public void Create_should_raise_domain_events()
    {
        // arrange & act
        var booking = TestBookingFactory.Create(
            id: BookingConstants.Booking1Id,
            timeProvider: DateTimeConstants.FakeProvider);
        
        // assert
        booking.DomainEvents.Count.ShouldBe(2);
        
        var created = booking.DomainEvents.OfType<BookingCreatedEvent>().Single();
        created.Id.ShouldBe(booking.Id);

        var pendingConfirmation = booking.DomainEvents.OfType<BookingPendingConfirmationEvent>().Single();
        pendingConfirmation.Id.ShouldBe(booking.Id);
        pendingConfirmation.OnDate.ShouldBe(DateTimeConstants.FakeProvider.GetLocalNow());
    }

    [Fact]
    public void Create_with_datetime_in_past_should_fail()
    {
        // arrange & act
        var action = () => TestBookingFactory.Create(
            date: DateTimeConstants.DateNow.AddDays(-1),
            timeProvider: DateTimeConstants.FakeProvider);
        
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