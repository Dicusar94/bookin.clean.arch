using BookingApp.Shared;

namespace BookingApp.BookingAggregate;

public interface IBookingRepository
{
    Task<Booking> AddBooking(Booking booking);
    Task<Booking> UpdateBooking(Booking booking);
    Task<Booking> GetBookingById(Guid id);

    Task<IReadOnlyList<Booking>> GetOverlappingBookingsAsync(
        Guid roomId,
        DateOnly date,
        TimeRange timeRange,
        CancellationToken ct = default);

    Task<bool> HasOverlappingUserBookingAsync(
        Guid userId,
        DateOnly date,
        TimeRange timeRange,
        CancellationToken ct);
}