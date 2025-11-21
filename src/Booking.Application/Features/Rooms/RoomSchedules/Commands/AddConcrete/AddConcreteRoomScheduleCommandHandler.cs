using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.Features.Rooms.RoomSchedules.Commons;
using BookingApp.RoomAggregate;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.RoomSchedules.Commands.AddConcrete;

public class AddConcreteRoomScheduleCommandHandler(
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) 
    : IRequestHandler<AddConcreteRoomScheduleCommand, RoomScheduleDto>
{
    public async Task<RoomScheduleDto> Handle(AddConcreteRoomScheduleCommand request, CancellationToken ct)
    {
        var room = await unitOfWork.Rooms.GetRoomById(request.Id, ct);

        var roomSchedule = RoomScheduleFactory.Concrete(
            roomId: room.Id,
            date: request.Date,
            timeRange: TimeRange.Create(request.Start, request.End),
            today: timeProvider.GetUtcNow().UtcDateTime);
        
        room.AddSchedule(roomSchedule);

        await unitOfWork.Rooms.UpdateRoom(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return roomSchedule.Convert();
    }
}