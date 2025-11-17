using BookingApp.BookingAggregate;
using BookingApp.BookingAggregate.Services;
using BookingApp.Features.Bookings.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Bookings.Commands.Create;

public class CreateBookingCommandHandler(
    TimeProvider timeProvider,
    IBookingPolicyService bookingPolicy,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken ct)
    {
        var booking = new BookingAggregate.Booking(
            roomId: request.RoomId,
            userId: request.UserId,
            date: request.Date,
            timeRange: TimeRange.Create(request.Start, request.End),
            timeProvider: timeProvider);

        await bookingPolicy.EnsureBookingCanBeCreatedAsync(booking, ct);
        
        await unitOfWork.Bookings.AddBooking(booking);
        await unitOfWork.SaveChangesAsync(ct);

        return booking.Convert();
    }
}