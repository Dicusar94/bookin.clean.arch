using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.AddRecurring;

public record AddRecurringRoomScheduleCommand(
    Guid Id,
    DayOfWeek DayOfWeek,
    TimeOnly Start,
    TimeOnly End) : IRequest<RoomSchedule>;