using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Queries.GetById;

public record GetRoomByIdQueryHandler(IRoomRepository roomRepository) : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken ct)
    {
       var result = await roomRepository.GetRoomById(request.id, ct);
       return result.Convert();
    }
}