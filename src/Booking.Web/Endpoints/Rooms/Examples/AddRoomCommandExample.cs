using BookingApp.Features.Rooms.Rooms.Commands.Add;
using Swashbuckle.AspNetCore.Filters;

namespace BookingApp.Endpoints.Rooms.Examples;

public class AddRoomCommandExample : IExamplesProvider<AddRoomCommand>
{
    public AddRoomCommand GetExamples() => new("Room1", 3);
}