namespace BookingApp.Messaging;

public static class RoutingKeys
{
    public static string EventTopicBase = "events";
    public static string NameEvents = $"{EventTopicBase}.name";
    public static string NameShout = $"{NameEvents}.shouted";
}