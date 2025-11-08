using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.Features.Rooms.RoomSchedules.Commons;
using BookingApp.RoomAggregate;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.AddRecurring;

public class AddRecurringRoomScheduleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddRecurringRoomScheduleCommand, RoomScheduleDto>
{
    public async Task<RoomScheduleDto> Handle(
        AddRecurringRoomScheduleCommand request, 
        CancellationToken ct)
    {
        var room = await unitOfWork.Rooms.GetRoomById(request.Id, ct);

        var roomSchedule = RoomScheduleFactory.Recurring(
            roomId: room.Id,
            dayOfWeek: request.DayOfWeek,
            timeRange: TimeRange.Create(request.Start, request.End));
        
        room.AddSchedule(roomSchedule);

        await unitOfWork.Rooms.UpdateRoom(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return roomSchedule.Convert();
    }
}