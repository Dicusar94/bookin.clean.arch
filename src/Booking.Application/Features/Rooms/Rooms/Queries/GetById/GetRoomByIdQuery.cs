using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Queries.GetById;

public record GetRoomByIdQuery(Guid id) : IRequest<Room>;