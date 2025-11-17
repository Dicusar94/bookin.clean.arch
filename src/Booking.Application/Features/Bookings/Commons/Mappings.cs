using BookingApp.BookingAggregate;
using BookingApp.Features.Rooms.Rooms.Commons;

namespace BookingApp.Features.Bookings.Commons;

public static class Mappings
{
    public static BookingStatusDto Convert(this BookingStatus entity)
    {
        return entity switch
        {
            BookingStatus.Pending => BookingStatusDto.Pending,
            BookingStatus.Canceled => BookingStatusDto.Canceled,
            BookingStatus.Confirmed => BookingStatusDto.Confirmed,
            _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, null)
        };
    }

    public static BookingDto Convert(this BookingAggregate.Booking entity)
    {
        return new BookingDto
        {
            RoomId = entity.RoomId,
            UserId = entity.UserId,
            Date = entity.Date,
            TimeRange = entity.TimeRange.Convert(),
            Status = entity.Status.Convert()
        };
    }
}