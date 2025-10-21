using BookingApp.RoomAggregate;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.AddConcrete;

public record AddConcreteRoomScheduleCommand(
    Guid Id,
    DateOnly Date,
    TimeOnly Start,
    TimeOnly End) : IRequest<RoomSchedule>;