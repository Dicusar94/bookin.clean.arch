using BookingApp.Features.Bookings.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Bookings.Commands.AutoCancel;

public class AutoCancelPendingBookingCommandHandler(IUnitOfWork unitOfWork, TimeProvider timeProvider) 
    : IRequestHandler<AutoCancelPendingBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(AutoCancelPendingBookingCommand request, CancellationToken ct)
    {
        var booking = await unitOfWork.Bookings.GetBookingById(request.Id);
        booking.AutoCancel(timeProvider);
        
        await unitOfWork.SaveChangesAsync(ct);
        return booking.Convert();
    }
}