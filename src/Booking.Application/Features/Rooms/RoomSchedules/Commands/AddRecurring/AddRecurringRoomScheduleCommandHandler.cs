using BookingApp.RoomAggregate;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.AddRecurring;

public class AddRecurringRoomScheduleCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddRecurringRoomScheduleCommand, RoomSchedule>
{
    public async Task<RoomSchedule> Handle(
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

        return roomSchedule;
    }
}