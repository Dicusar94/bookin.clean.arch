using System.Diagnostics;
using BookingApp.BookingAggregate;
using BookingApp.Exceptions;
using BookingApp.Shared;
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

    public async Task<IReadOnlyList<Booking>> GetOverlappingBookingsAsync(
        Guid roomId, 
        DateOnly date, 
        TimeRange timeRange, 
        CancellationToken ct = default)
    {
        return await context.Bookings
            .Where(x => 
                x.Status != BookingStatus.Canceled &&
                x.RoomId == roomId &&
                x.Date == date &&
                x.TimeRange.Start < timeRange.End &&
                x.TimeRange.End > timeRange.Start)
            .ToListAsync(ct);
    }

    public async Task<bool> HasOverlappingUserBookingAsync(
        Guid userId,
        DateOnly date,
        TimeRange timeRange,
        CancellationToken ct)
    {
        return await context.Bookings
            .AnyAsync(x =>
                x.Status != BookingStatus.Canceled &&
                x.UserId == userId &&
                x.Date == date &&
                x.TimeRange.Start < timeRange.End &&
                x.TimeRange.End > timeRange.Start, 
                ct);
    }
}