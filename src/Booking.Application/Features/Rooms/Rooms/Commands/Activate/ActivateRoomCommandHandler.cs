using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Commands.Activate;

public class ActivateRoomCommandHandler(IUnitOfWork unitOfWork, TimeProvider timeProvider) 
    : IRequestHandler<ActivateRoomCommand, RoomDto>
{
    public async Task<RoomDto> Handle(ActivateRoomCommand request, CancellationToken ct)
    {
        var room = await unitOfWork.Rooms.GetRoomById(request.Id, ct);
        room.Activate(timeProvider);

        await unitOfWork.Rooms.UpdateRoom(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return room.Convert();
    }
}