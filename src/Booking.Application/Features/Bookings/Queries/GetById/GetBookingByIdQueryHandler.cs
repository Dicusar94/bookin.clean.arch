using BookingApp.Features.Bookings.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Bookings.Queries.GetById;

public class GetBookingByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetBookingByIdQuery, BookingDto>
{
    public async Task<BookingDto> Handle(GetBookingByIdQuery request, CancellationToken ct)
    {
        var booking = await unitOfWork.Bookings.GetBookingById(request.Id);
        return booking.Convert();
    }
}