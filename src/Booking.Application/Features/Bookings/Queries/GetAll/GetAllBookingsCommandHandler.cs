using BookingApp.Features.Bookings.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Bookings.Queries.GetAll;

public class GetAllBookingsCommandHandler(IUnitOfWork unitOfWork) 
    : IRequestHandler<GetAllBookingsCommand, IEnumerable<BookingDto>>
{
    public async Task<IEnumerable<BookingDto>> Handle(GetAllBookingsCommand request, CancellationToken ct)
    {
        var result = await unitOfWork.Bookings.GetBookings(ct);
        return result.Select(x => x.Convert());
    }
}