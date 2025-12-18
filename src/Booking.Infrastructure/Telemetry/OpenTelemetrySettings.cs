namespace Booking.Infrastructure.Telemetry;

public class OpenTelemetrySettings
{
    public required Uri TracesEndpoint { get; init; }
    
    public List<TelemetryAttribute> Attributes { get; init; } = [];

    public bool Enabled { get; set; }
    
    public List<KeyValuePair<string, object>> GetAttributesAsKeyValuePairs()
    {
        return [.. Attributes.Select(attr => new KeyValuePair<string, object>(attr.Key, attr.Value))];
    }

    public class TelemetryAttribute
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}