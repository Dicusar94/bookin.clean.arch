using Asp.Versioning;
using BookingApp.Features.Rooms.Rooms.Commands.Activate;
using BookingApp.Features.Rooms.Rooms.Commands.Add;
using BookingApp.Features.Rooms.Rooms.Commands.Deactivate;
using BookingApp.Features.Rooms.Rooms.Commons;
using BookingApp.Features.Rooms.Rooms.Queries.GetAll;
using BookingApp.Features.Rooms.Rooms.Queries.GetById;
using BookingApp.Infrastructure.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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
            .Produces<RoomDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("AddRoom");
        
        group.MapGet("{id:guid}", GetRoomById)
            .Produces<RoomDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("GetRoomById");
        
        group.MapGet(string.Empty, GetRooms)
            .Produces<List<RoomDto>>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("GetRooms");

        group.MapPut("{id:guid}/deactivate", DeactivateRoom)
            .Produces<RoomDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("DeactivateRoom");
        
        group.MapPut("{id:guid}/activate", ActivateRoom)
            .Produces<RoomDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("ActivateRoom");
    }

    private static async Task<IResult> GetRooms([FromServices] ISender sender)
    {
        var result = await sender.Send(new GetRoomsQuery());
        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetRoomById(Guid id, [FromServices] ISender sender)
    {
        var query = new GetRoomByIdQuery(id);
        var room = await sender.Send(query);
        return Results.Ok(room);
    }

    private static async Task<IResult> DeactivateRoom(Guid id, [FromServices] ISender sender)
    {
        var command = new DeactivateRoomCommand(id);
        var room = await sender.Send(command);
        return Results.Ok(room);
    }
    
    private static async Task<IResult> ActivateRoom(Guid id, [FromServices] ISender sender)
    {
        var command = new ActivateRoomCommand(id);
        var room = await sender.Send(command);
        return Results.Ok(room);
    }
    
    private static async Task<IResult> AddRoom(AddRoomCommand command, [FromServices] ISender sender)
    {
        var room = await sender.Send(command);
        return Results.Ok(room);
    }
}
