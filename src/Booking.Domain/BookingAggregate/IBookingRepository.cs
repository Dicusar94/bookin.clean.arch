namespace BookingApp.BookingAggregate;

public interface IBookingRepository
{
    Task<Booking> AddBooking(Booking booking);
    Task<Booking> UpdateBooking(Booking booking);
    Task<Booking> GetBookingById(Guid id);
}