using Microsoft.Extensions.Time.Testing;

namespace BookingApp.Utils.TestContants.Schared;

public class DateTimeConstants
{
    
    public static readonly DateOnly DateNow = new(2025, 10, 12);
    
    public static readonly DateTime DateTimeNow = new(
        date: DateNow,
        time: new TimeOnly(0, 0));
    
    public static DayOfWeek DayOfWeek => DateNow.DayOfWeek;

    public static readonly DateTimeOffset DateTimeOffsetNow = new(DateTimeNow, TimeSpan.Zero);

    public static readonly FakeTimeProvider TimeProvider = new(DateTimeOffsetNow);
}