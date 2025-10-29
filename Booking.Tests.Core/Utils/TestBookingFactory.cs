using BookingApp.BookingAggregate;
using BookingApp.Shared;
using BookingApp.Utils.TestContants.Bookings;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using Microsoft.Extensions.Time.Testing;

namespace BookingApp.Utils;

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
            timeRange: timeRange ?? TimeRangeConstants.NineAmToElevenAm,
            id: id ?? BookingConstants.Id);
    }
}