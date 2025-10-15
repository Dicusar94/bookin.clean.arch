using BookingApp.Shared;

namespace Booking.Tests.Unit.Utils.TestContants;

public static class TestBookingFactory
{
    public static BookingApp.BookingAggregate.Booking Create(
        Guid? roomId,
        Guid? userId,
        DateOnly? date,
        TimeRange? timeRange,
        TimeProvider? timeProvider,
        Guid? id = null
    )
    {
        return default;
    }
}