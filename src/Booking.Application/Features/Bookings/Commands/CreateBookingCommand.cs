using BookingApp.Features.Bookings.Commons;
using MediatR;

namespace BookingApp.Features.Bookings.Commands;

public record CreateBookingCommand(
    Guid RoomId,
    Guid UserId,
    DateOnly Date,
    TimeOnly Start,
    TimeOnly End) : IRequest<BookingDto>;