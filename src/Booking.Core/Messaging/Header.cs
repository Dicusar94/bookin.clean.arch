using System.Text;

namespace Booking.Core.Messaging;

public record Header
{
    public IDictionary<string, object>? Properties { get; set; }
    public string SourceCode => RetrieveStringHeader(HeaderConstants.SourceCode);
    public string EventCode => RetrieveStringHeader(HeaderConstants.EventCode);
    
    public Header()
    {
        Properties = new Dictionary<string, object>();
    }

    /// <summary>
    /// Add additional information to the header.
    /// </summary>
    /// <param name="sourceCode">The name of the app that pushed the message.</param>
    /// <param name="eventCode"></param>
    public Header(string sourceCode, string eventCode)
    {
        Properties = new Dictionary<string, object>
        {
            {HeaderConstants.SourceCode, sourceCode},
            {HeaderConstants.EventCode, eventCode}
        };
    }
    
    private string RetrieveStringHeader(string key)
    {
        var value = Properties!.FirstOrDefault(x => x.Key == key).Value;

        if (value is null)
        {
            throw new InvalidOperationException("NotFound");
        }

        if (value is byte[] byteArray)
        {
            return Encoding.UTF8.GetString(byteArray);
        }

        if (value is string stringValue)
        {
            return stringValue;
        }

        return string.Empty;
    }
}