using BookingApp.BookingAggregate;
using BookingApp.RoomAggregate;
using BookingApp.Shared;

namespace BookingApp.Persistence.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public IRoomRepository Rooms { get; } = new RoomRepository(context);
    public IBookingRepository Bookings { get; } = new BookingRepository(context);

    public Task SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}