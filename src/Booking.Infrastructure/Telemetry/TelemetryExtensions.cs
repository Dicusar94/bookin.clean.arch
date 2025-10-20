using System.Diagnostics;
using BookingApp.BookingAggregate;

namespace BookingApp.Telemetry;

public static class TelemetryExtensions
{
    public static Activity? SetBooking(this Activity? activity, Booking booking)
    {
        return activity?
            .SetRoomId(booking.RoomId)
            .SetUserId(booking.UserId)
            .SetBookingId(booking.Id)
            ?.SetTag("Date", booking.Date)
            .SetTag("TimeRange", booking.TimeRange);
    }
}