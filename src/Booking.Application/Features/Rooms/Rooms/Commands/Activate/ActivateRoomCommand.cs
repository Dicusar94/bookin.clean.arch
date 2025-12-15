using BookingApp.Features.Rooms.Rooms.Commons;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Commands.Activate;

public record ActivateRoomCommand(Guid Id) : IRequest<RoomDto>;