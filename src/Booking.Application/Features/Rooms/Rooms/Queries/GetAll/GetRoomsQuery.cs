using BookingApp.Features.Rooms.Rooms.Commons;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Queries.GetAll;

public record GetRoomsQuery : IRequest<List<RoomDto>>;