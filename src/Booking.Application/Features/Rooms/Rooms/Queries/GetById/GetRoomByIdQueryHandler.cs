using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Queries.GetById;

public record GetRoomByIdQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetRoomByIdQuery, Room>
{
    public Task<Room> Handle(GetRoomByIdQuery request, CancellationToken ct) 
        => roomRepository.GetRoomById(request.id, ct);
}