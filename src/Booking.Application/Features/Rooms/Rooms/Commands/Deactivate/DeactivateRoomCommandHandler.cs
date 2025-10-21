using BookingApp.RoomAggregate;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Commands.Deactivate;

public class DeactivateRoomCommandHandler(
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) 
    : IRequestHandler<DeactivateRoomCommand, Room>
{
    public async Task<Room> Handle(DeactivateRoomCommand request, CancellationToken ct)
    {
        var room = await unitOfWork.Rooms.GetRoomById(request.Id, ct);
        room.Deactivate(timeProvider);

        await unitOfWork.Rooms.UpdateRoom(room, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return room;
    }
}