using BookingApp.Features.Bookings.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Bookings.Commands.Confirm;

public class ConfirmBookingCommandHandler(IUnitOfWork unitOfWork, TimeProvider timeProvider) 
    : IRequestHandler<ConfirmBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(ConfirmBookingCommand request, CancellationToken ct)
    {
        var booking = await unitOfWork.Bookings.GetBookingById(request.Id);
        booking.Confirm(timeProvider);
        await unitOfWork.SaveChangesAsync(ct);

        return booking.Convert();
    }
}