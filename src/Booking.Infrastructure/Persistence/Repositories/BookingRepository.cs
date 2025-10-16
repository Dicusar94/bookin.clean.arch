using BookingApp.BookingAggregate;

namespace BookingApp.Persistence.Repositories;

public class BookingRepository(ApplicationDbContext context) : IBookingRepository
{
    public Task<Booking> AddBooking(Booking booking)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> UpdateBooking(Booking booking)
    {
        throw new NotImplementedException();
    }

    public Task<Booking> GetBookingById(Guid id)
    {
        throw new NotImplementedException();
    }
}