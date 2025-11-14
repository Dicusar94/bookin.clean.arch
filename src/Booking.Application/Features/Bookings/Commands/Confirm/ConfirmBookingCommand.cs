using BookingApp.Features.Bookings.Commons;
using MediatR;

namespace BookingApp.Features.Bookings.Commands.Confirm;

public record ConfirmBookingCommand(Guid Id) : IRequest<BookingDto>;