using BookingApp.Shared;

namespace BookingApp.Utils.TestContants.Schared;

public class TimeRangeConstants
{
    
    public static TimeRange NineAmToElevenAm => TimeRange.Create(
        start: new TimeOnly(9, 0),
        end: new TimeOnly(11, 0));
    
    public static TimeRange TenAmToTwelvePm => TimeRange.Create(
        start: new TimeOnly(10, 0),
        end: new TimeOnly(12, 0));
   
    public static TimeRange ElevenAmToTwelvePm => TimeRange.Create(
        start: new TimeOnly(11, 0),
        end: new TimeOnly(12, 0));
    
    public static TimeRange EightAmToFivePm => TimeRange.Create(
        start: new TimeOnly(8, 0),
        end: new TimeOnly(17, 0));
    
    public static TimeRange FourAmToFivePm => TimeRange.Create(
        start: new TimeOnly(16, 0),
        end: new TimeOnly(17, 0));
}