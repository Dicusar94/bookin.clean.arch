using BookingApp.Features.Bookings.Commons;
using MediatR;

namespace BookingApp.Features.Bookings.Commands.AutoCancel;

public record AutoCancelPendingBookingCommand(Guid Id) : IRequest<BookingDto>;