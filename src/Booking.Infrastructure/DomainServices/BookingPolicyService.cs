using BookingApp.BookingAggregate;
using BookingApp.BookingAggregate.Services;
using BookingApp.RoomAggregate;

namespace BookingApp.DomainServices;

public class BookingPolicyService(
    IRoomRepository roomRepository, 
    IBookingRepository bookingRepository) : IBookingPolicyService
{
    public async Task EnsureBookingCanBeCreatedAsync(Booking booking, CancellationToken ct = default)
    {
        var room = await roomRepository.GetRoomById(booking.RoomId);
        
        
    }
}