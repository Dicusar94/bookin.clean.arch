using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.Reschedule;

public record RescheduleCommand(
    Guid Id,
    Guid ScheduleId,
    TimeOnly Start,
    TimeOnly End) : IRequest<RoomSchedule>;