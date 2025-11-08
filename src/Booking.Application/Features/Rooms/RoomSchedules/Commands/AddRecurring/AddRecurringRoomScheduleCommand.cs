using BookingApp.Features.Rooms.RoomSchedules.Commons;
using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.AddRecurring;

public record AddRecurringRoomScheduleCommand(
    Guid Id,
    DayOfWeek DayOfWeek,
    TimeOnly Start,
    TimeOnly End) : IRequest<RoomScheduleDto>;