using Asp.Versioning;
using BookingApp.Features.Bookings.Commands.Cancel;
using BookingApp.Features.Bookings.Commands.Confirm;
using BookingApp.Features.Bookings.Commands.Create;
using BookingApp.Features.Bookings.Commons;
using BookingApp.Features.Bookings.Queries.GetAll;
using BookingApp.Features.Bookings.Queries.GetById;
using BookingApp.Infrastructure.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static BookingApp.Infrastructure.Endpoints.Constants.ContentTypes;

namespace BookingApp.Endpoints.Bookings;

public class BookingsEndpoints : IEndpointsDefinition
{
    public static void ConfigureEndpoints(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("api/v{version:apiVersion}/bookings")
            .WithTags("bookings")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .ProducesProblem(statusCode: 400);

        group.MapPost(string.Empty, CreateBooking)
            .Accepts<CreateBookingRequest>(ApplicationJson)
            .Produces<BookingDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("CreateBooking");

        group.MapPut("{id:guid}/cancel", CancelBooking)
            .Produces<BookingDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("CancelBooking");
        
        group.MapPut("{id:guid}/confirm", ConfirmBooking)
            .Produces<BookingDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("ConfirmBooking");
        
        group.MapGet(string.Empty, GetAllBookings)
            .Produces<IEnumerable<BookingDto>>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("GetAllBookings");
        
        group.MapGet("{id:guid}", GetBookingById)
            .Produces<BookingDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("GetBookingById");
    }

    private static async Task<IResult> CreateBooking(CreateBookingRequest request, [FromServices] ISender sender)
    {
        var command = new CreateBookingCommand(
            RoomId: request.RoomId,
            UserId: request.UserId,
            Date: request.Date,
            Start: request.Start,
            End: request.End);

        var booking = await sender.Send(command);
        return Results.Ok(booking);
    }

    private static async Task<IResult> GetAllBookings([FromServices] ISender sender)
    {
        var query = new GetAllBookingsCommand();
        var result = await sender.Send(query);
        return Results.Ok(result);
    }
    
    private static async Task<IResult> GetBookingById(Guid id, [FromServices] ISender sender)
    {
        var query = new GetBookingByIdQuery(id);
        var result = await sender.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> CancelBooking(Guid id, [FromServices] ISender sender)
    {
        var command = new CancelBookingCommand(id);
        var result = await sender.Send(command);
        return Results.Ok(result);
    }
    
    private static async Task<IResult> ConfirmBooking(Guid id, [FromServices] ISender sender)
    {
        var command = new ConfirmBookingCommand(id);
        var result = await sender.Send(command);
        return Results.Ok(result);
    }

    private sealed record CreateBookingRequest(
        Guid RoomId,
        Guid UserId,
        DateOnly Date,
        TimeOnly Start,
        TimeOnly End);
}