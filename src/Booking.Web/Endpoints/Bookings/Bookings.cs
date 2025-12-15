using Asp.Versioning;
using BookingApp.Features.Bookings.Commands.Create;
using BookingApp.Features.Bookings.Commons;
using BookingApp.Infrastructure.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static BookingApp.Infrastructure.Endpoints.Constants.ContentTypes;

namespace BookingApp.Endpoints.Bookings;

public class Bookings : IEndpointsDefinition
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

    private sealed record CreateBookingRequest(
        Guid RoomId,
        Guid UserId,
        DateOnly Date,
        TimeOnly Start,
        TimeOnly End);
}