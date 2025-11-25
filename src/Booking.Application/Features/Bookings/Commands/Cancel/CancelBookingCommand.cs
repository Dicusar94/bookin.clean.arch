using BookingApp.Features.Bookings.Commons;
using MediatR;

namespace BookingApp.Features.Bookings.Commands.Cancel;

public record CancelBookingCommand(Guid Id) : IRequest<BookingDto>;