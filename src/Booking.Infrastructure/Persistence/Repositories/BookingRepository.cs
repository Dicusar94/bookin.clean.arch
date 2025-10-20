using System.Diagnostics;
using BookingApp.BookingAggregate;
using BookingApp.Exceptions;
using BookingApp.Telemetry;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Persistence.Repositories;

public class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public async Task<Booking> AddBooking(Booking booking)
    {
        _ = RunTimeDiagnosticConfig.Source.StartActivity()
            .SetBooking(booking);
        
        await context.Bookings.AddAsync(booking);
        
        return booking;
    }

    public Task<Booking> UpdateBooking(Booking booking)
    {
        _ = RunTimeDiagnosticConfig.Source.StartActivity()
            .SetBooking(booking);
        
        context.Bookings.Update(booking);
        
        return Task.FromResult(booking);
    }

    public async Task<Booking> GetBookingById(Guid id)
    {
        var dbActivity = RunTimeDiagnosticConfig.Source.StartActivity()
            .SetBookingId(id);
        
        var booking = await context.Bookings.FirstOrDefaultAsync(x => x.Id == id);

        if (booking is null)
        {
            var exception =  new EntityNotFoundException(nameof(Booking), id.ToString());
            dbActivity.AddExceptionAndFail(exception);
            throw exception;
        }

        return booking;
    }
}

file static class Extensions
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