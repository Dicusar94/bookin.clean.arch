using BookingApp.BookingAggregate;
using BookingApp.Shared;
using BookingApp.Tests.Unit.Utils.TestContants.Bookings;
using BookingApp.Tests.Unit.Utils.TestContants.Rooms;
using BookingApp.Tests.Unit.Utils.TestContants.Schared;
using BookingApp.Tests.Unit.Utils.TestContants.Users;
using Microsoft.Extensions.Time.Testing;

namespace BookingApp.Tests.Unit.Utils;

public static class TestBookingFactory
{
    public static Booking Create(
        Guid? roomId = null,
        Guid? userId = null,
        DateOnly? date = null,
        TimeRange? timeRange = null,
        TimeProvider? timeProvider = null,
        Guid? id = null
    )
    {
        return new Booking(
            roomId: roomId ?? RoomConstants.Id,
            userId: userId ?? UserConstants.Id,
            date: date ?? DateTimeConstants.DateNow,
            timeProvider: timeProvider ?? new FakeTimeProvider(new DateTimeOffset(DateTimeConstants.DateTimeNow)),
            timeRange: timeRange ?? TimeRangeConstants.NineToEleven,
            id: id ?? BookingConstants.Id);
    }
}