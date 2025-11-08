using BookingApp.Features.Rooms.RoomSchedules.Commons;
using BookingApp.RoomAggregate;
using BookingApp.Shared;

namespace BookingApp.Features.Rooms.Rooms.Commons;

public static class Mappings
{
    public static RoomDto Convert(this Room entity)
    {
        return new RoomDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Capacity = entity.Capacity,
            Status = entity.Status.Convert(),
            Schedules = entity.Schedules.Select(x => x.Convert()).ToList()
        };
    }

    public static RoomScheduleDto Convert(this RoomSchedule entity)
    {
        return new RoomScheduleDto
        {
            RoomId = entity.RoomId,
            DayOfWeek = entity.DayOfWeek,
            IsRecurring = entity.IsRecurring,
            TimeRange = entity.TimeRange.Convert(),
            Date = entity.Date 
        };
    }

    public static TimeRangeDto Convert(this TimeRange entity)
    {
        return new TimeRangeDto
        {
            Start = entity.Start,
            End = entity.End 
        };
    }

    public static RoomStatusDto Convert(this RoomStatus entity)
    {
        return entity switch
        {
            RoomStatus.Active => RoomStatusDto.Active,
            RoomStatus.Inactive => RoomStatusDto.Inactive,
            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
        };
    }
}