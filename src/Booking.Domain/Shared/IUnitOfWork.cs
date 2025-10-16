using BookingApp.BookingAggregate;
using BookingApp.RoomAggregate;

namespace BookingApp.Shared;

public interface IUnitOfWork
{
    IRoomRepository Rooms { get; }
    IBookingRepository Bookings { get; } 
    Task SaveChangesAsync(CancellationToken cancellationToken);
}