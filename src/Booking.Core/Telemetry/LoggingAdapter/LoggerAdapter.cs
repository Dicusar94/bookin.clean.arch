using Microsoft.Extensions.Logging;

namespace BookingApp.Telemetry.LoggingAdapter;

public class LoggerAdapter<T>(ILogger<T> logger) : ILoggerAdapter<T>
{
    public void LogInformation(string messageTemplate, params object?[] args)
    {
        logger.LogInformation(messageTemplate, args);
    }
}