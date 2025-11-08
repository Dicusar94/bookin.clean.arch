using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.Features.Rooms.RoomSchedules.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.Reschedule;

public class RescheduleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RescheduleCommand, RoomScheduleDto>
{
    public async Task<RoomScheduleDto> Handle(RescheduleCommand request, CancellationToken ct)
    {
        var room = await unitOfWork.Rooms.GetRoomById(request.Id, ct);
        
        var roomSchedule = room.Reschedule(
            roomScheduleId: request.ScheduleId, 
            newTimeRange: TimeRange.Create(request.Start, request.End));

        await unitOfWork.Rooms.UpdateRoom(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return roomSchedule.Convert();
    }
}