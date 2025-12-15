using BookingApp.Features.Bookings.Commons;
using MediatR;

namespace BookingApp.Features.Bookings.Queries.GetAll;

public record GetAllBookingsCommand : IRequest<IEnumerable<BookingDto>>;