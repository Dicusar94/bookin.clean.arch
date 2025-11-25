using BookingApp.Features.Bookings.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Bookings.Commands.Cancel;

public class CancelBookingCommandHandler(
    IUnitOfWork unitOfWork, 
    TimeProvider timeProvider) 
    : IRequestHandler<CancelBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CancelBookingCommand request, CancellationToken ct)
    {
        var booking = await unitOfWork.Bookings.GetBookingById(request.Id);
        booking.Cancel(timeProvider);
        await unitOfWork.SaveChangesAsync(ct);

        return booking.Convert();
    }
}