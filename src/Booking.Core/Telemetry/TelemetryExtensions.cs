using System.Diagnostics;

namespace BookingApp.Telemetry;

public static class TelemetryExtensions
{
    public static Activity? AddExceptionAndFail(this Activity? activity, Exception exception)
    {
        activity?.AddException(exception);
        activity?.SetStatus(ActivityStatusCode.Error);
        return activity;
    }
    
    public static Activity? SetUserId(this Activity? activity, string id)
    {
        activity?.SetTag(GlobalOTelTags.UserId, id);
        return activity;
    }
    
    public static Activity? SetDateTimeOffset(this Activity? activity, DateTimeOffset offset)
    {
        activity?.SetTag(GlobalOTelTags.UtcDateTimeOffset, offset);
        return activity;
    }

    public static Activity? SetRoutingKey(this Activity? activity, string routingKey)
    {
        activity?.SetTag("routingKey", routingKey);
        return activity;
    }

    public static Activity? SetRoomId(this Activity? activity, Guid roomId)
    {
        activity?.SetTag(GlobalOTelTags.RoomId, roomId);
        return activity;
    }
    
    public static Activity? SetRoomName(this Activity? activity, string roomName)
    {
        activity?.SetTag(GlobalOTelTags.RoomName, roomName);
        return activity;
    }
    
    public static Activity? SetRoomStatus(this Activity? activity, string roomStatus)
    {
        activity?.SetTag(GlobalOTelTags.RoomStatus, roomStatus);
        return activity;
    }
    
    // TODO: add rest of OTel tags.
}