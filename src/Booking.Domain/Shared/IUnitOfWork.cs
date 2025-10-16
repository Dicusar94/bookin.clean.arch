using BookingApp.BookingAggregate;

namespace BookingApp.Shared;

public interface IUnitOfWork
{
    IBookingRepository Booking { get; } 
}