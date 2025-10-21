using BookingApp.RoomAggregate;
using BookingApp.Shared;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Commands.Add;

public class AddRoomCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddRoomCommand, Room> 
{
    public async Task<Room> Handle(AddRoomCommand request, CancellationToken ct)
    {
        var room = new Room(request.Name, request.Capacity);
        
        await unitOfWork.Rooms.AddRoom(room, ct);
        await unitOfWork.SaveChangesAsync(ct);
        
        return room;
    }
}