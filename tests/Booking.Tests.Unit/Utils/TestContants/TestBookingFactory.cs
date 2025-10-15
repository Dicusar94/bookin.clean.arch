using Booking.Domain.Shared;

namespace Booking.Tests.Unit.Utils.TestContants;

public static class TestBookingFactory
{
    public static Domain.BookingAggregate.Booking Create(
        Guid? roomId,
        Guid? userId,
        DateOnly? date,
        TimeRange? timeRange,
        TimeProvider? timeProvider,
        Guid? id = null
    )
    {
        return new Domain.BookingAggregate.Booking(
            roomId: roomId ?? RoomConstants.Id,
            userId: userId ?? UserConstants.Id,
        );
    }
}