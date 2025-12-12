using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Queries.GetAll;

public class GetRoomsQueryHandler(IRoomRepository repository) : IRequestHandler<GetRoomsQuery, List<RoomDto>>
{
    public async Task<List<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        var result  = await repository.GetRooms(cancellationToken);
        return result.Select(x => x.Convert()).ToList();
    }
}