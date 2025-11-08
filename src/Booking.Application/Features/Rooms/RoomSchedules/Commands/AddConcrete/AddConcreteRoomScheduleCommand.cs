using BookingApp.Features.Rooms.RoomSchedules.Commons;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.AddConcrete;

public record AddConcreteRoomScheduleCommand(
    Guid Id,
    DateOnly Date,
    TimeOnly Start,
    TimeOnly End) : IRequest<RoomScheduleDto>;