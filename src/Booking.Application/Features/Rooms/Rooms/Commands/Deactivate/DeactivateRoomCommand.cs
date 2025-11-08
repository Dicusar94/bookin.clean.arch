using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Commands.Deactivate;

public record DeactivateRoomCommand(Guid Id) : IRequest<RoomDto>;