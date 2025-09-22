using System.Text.Json;

namespace Booking.Core.Messaging;

public record Message
{
    public string Id { get; }
    public Header Header { get; }
    public string Body { get;}
    
    public Message(Header header, object body)
    {
        Header = header;
        Body = JsonSerializer.Serialize(body);
        Id = Guid.NewGuid().ToString();
    }
    
    public Message(Header header, string serializedObject)
    {
        Header = header;
        Body = serializedObject;
        Id = Guid.NewGuid().ToString();
    }
}