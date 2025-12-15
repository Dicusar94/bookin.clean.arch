using BookingApp.Features.Bookings.Commons;
using MediatR;

namespace BookingApp.Features.Bookings.Queries.GetById;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDto>;