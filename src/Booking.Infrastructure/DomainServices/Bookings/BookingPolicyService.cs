using BookingApp.BookingAggregate;
using BookingApp.BookingAggregate.Services;
using BookingApp.RoomAggregate;
using BookingApp.Telemetry;

namespace BookingApp.DomainServices.Bookings;

public class BookingPolicyService(
    IRoomRepository roomRepository, 
    IBookingRepository bookingRepository) : IBookingPolicyService
{
    public async Task EnsureBookingCanBeCreatedAsync(BookingAggregate.Booking booking, CancellationToken ct = default)
    {
        var activity = RunTimeDiagnosticConfig.Source.StartActivity()
            .SetBooking(booking);
        
        var room = await roomRepository.GetRoomById(booking.RoomId, ct);

        if (room.Status is not RoomStatus.Active)
        {
            throw new Exception("Room is not active at the moment");
        }
        
        if (!room.IsAvailableOn(booking.Date, booking.TimeRange))
        {
            var exception = new Exception("Room not available during the requested time");
            activity.AddExceptionAndFail(exception);
            throw exception;
        }

        var overlappingBookings = await bookingRepository.GetOverlappingBookingsAsync(
            booking.RoomId,
            booking.Date,
            booking.TimeRange,
            ct);

        if (overlappingBookings.Count >= room.Capacity)
        {
            var exception = new Exception($"Room capacity of {room.Capacity} exceeded for the requested time");
            activity.AddExceptionAndFail(exception);
            throw exception;
        }

        var userHasOverlaps = await bookingRepository.HasOverlappingUserBookingAsync(
            booking.UserId,
            booking.Date,
            booking.TimeRange,
            ct);

        if (userHasOverlaps)
        {
            var exception = new Exception($"User already has overlapping bookings");
            activity.AddExceptionAndFail(exception);
            throw exception;
        }
    }
}