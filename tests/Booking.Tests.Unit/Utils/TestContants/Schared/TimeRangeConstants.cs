using BookingApp.Shared;

namespace BookingApp.Tests.Unit.Utils.TestContants.Schared;

public class TimeRangeConstants
{
    
    public static readonly TimeRange NineToEleven = TimeRange.Create(
        start: new TimeOnly(9, 0),
        end: new TimeOnly(11, 0));
    
    public static readonly TimeRange TenToTwelve = TimeRange.Create(
        start: new TimeOnly(10, 0),
        end: new TimeOnly(12, 0));
    
    public static readonly TimeRange ElevenToTwelve = TimeRange.Create(
        start: new TimeOnly(11, 0),
        end: new TimeOnly(12, 0));
}