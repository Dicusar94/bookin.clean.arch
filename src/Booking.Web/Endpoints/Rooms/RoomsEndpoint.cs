using Asp.Versioning;
using BookingApp.Features.Rooms.Rooms.Commands.Add;
using BookingApp.Infrastructure.Endpoints;
using BookingApp.RoomAggregate;
using static BookingApp.Infrastructure.Endpoints.Constants.ContentTypes;

namespace BookingApp.Endpoints.Rooms;

public class RoomsEndpoint : IEndpointsDefinition
{
    public static void ConfigureEndpoints(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("api/v{version:apiVersion}/rooms/")
            .WithTags("rooms")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .ProducesProblem(statusCode: 400);
            //.RequireAuthorization()

        group.MapPost(string.Empty, AddRoom)
            .Accepts<AddRoomCommand>(ApplicationJson)
            .Produces<Room>(statusCode: 201, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("AddRoom");
    }

    private static async Task<IResult> AddRoom()
    {
        return Results.Created();
    }
    
}