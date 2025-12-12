using Asp.Versioning;
using BookingApp.Features.Rooms.RoomSchedules.Commands.AddConcrete;
using BookingApp.Features.Rooms.RoomSchedules.Commands.AddRecurring;
using BookingApp.Features.Rooms.RoomSchedules.Commands.Reschedule;
using BookingApp.Features.Rooms.RoomSchedules.Commons;
using BookingApp.Infrastructure.Endpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static BookingApp.Infrastructure.Endpoints.Constants.ContentTypes;

namespace BookingApp.Endpoints.Rooms;

public class RoomSchedulesEndpoint : IEndpointsDefinition
{
    public static void ConfigureEndpoints(IEndpointRouteBuilder app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1.0))
            .ReportApiVersions()
            .Build();

        var group = app.MapGroup("api/v{version:apiVersion}/rooms/{id:guid}/schedules/")
            .WithTags("room-schedules")
            .WithApiVersionSet(versionSet)
            .MapToApiVersion(1.0)
            .ProducesProblem(statusCode: 400);

        group.MapPost("concrete", AddConcreteRoomSchedule)
            .Accepts<AddConcreteRoomScheduleRequest>(ApplicationJson)
            .Produces<RoomScheduleDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("AddConcreteRoomSchedule");

        group.MapPost("recurring", AddRecurringRoomSchedule)
            .Accepts<AddRecurringRoomScheduleRequest>(ApplicationJson)
            .Produces<RoomScheduleDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("AddRecurringRoomSchedule");

        group.MapPut("{scheduleId:guid}", RescheduleRoomSchedule)
            .Accepts<RescheduleRoomScheduleRequest>(ApplicationJson)
            .Produces<RoomScheduleDto>(statusCode: 200, ApplicationJson)
            .ProducesValidationProblem()
            .WithName("RescheduleRoomSchedule");
    }

    private static async Task<IResult> AddConcreteRoomSchedule(
        Guid id,
        AddConcreteRoomScheduleRequest request,
        [FromServices] ISender sender)
    {
        var command = new AddConcreteRoomScheduleCommand(id, request.Date, request.Start, request.End);
        var schedule = await sender.Send(command);
        return Results.Ok(schedule);
    }

    private static async Task<IResult> AddRecurringRoomSchedule(
        Guid id,
        AddRecurringRoomScheduleRequest request,
        [FromServices] ISender sender)
    {
        var command = new AddRecurringRoomScheduleCommand(id, request.DayOfWeek, request.Start, request.End);
        var schedule = await sender.Send(command);
        return Results.Ok(schedule);
    }

    private static async Task<IResult> RescheduleRoomSchedule(
        Guid id,
        Guid scheduleId,
        RescheduleRoomScheduleRequest request,
        [FromServices] ISender sender)
    {
        var command = new RescheduleCommand(id, scheduleId, request.Start, request.End);
        var schedule = await sender.Send(command);
        return Results.Ok(schedule);
    }

    private sealed record AddConcreteRoomScheduleRequest(DateOnly Date, TimeOnly Start, TimeOnly End);

    private sealed record AddRecurringRoomScheduleRequest(DayOfWeek DayOfWeek, TimeOnly Start, TimeOnly End);

    private sealed record RescheduleRoomScheduleRequest(TimeOnly Start, TimeOnly End);
}
