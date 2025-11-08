using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Commands.Deactivate;

public class DeactivateRoomCommandHandler(
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) 
    : IRequestHandler<DeactivateRoomCommand, RoomDto>
{
    public async Task<RoomDto> Handle(DeactivateRoomCommand request, CancellationToken ct)
    {
        var room = await unitOfWork.Rooms.GetRoomById(request.Id, ct);
        room.Deactivate(timeProvider);

        await unitOfWork.Rooms.UpdateRoom(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return room.Convert();
    }
}