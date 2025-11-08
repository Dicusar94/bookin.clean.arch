using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.RoomAggregate;
using MediatR;

namespace BookingApp.Features.Rooms.Rooms.Commands.Add;

public record AddRoomCommand(string Name, int Capacity) : IRequest<RoomDto>;