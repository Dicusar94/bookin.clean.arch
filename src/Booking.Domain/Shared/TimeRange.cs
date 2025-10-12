using Booking.Core.Entities;

namespace Booking.Domain.Shared;

public class TimeRange : ValueObject
{
    public TimeOnly Start { get; private init; }
    public TimeOnly End { get; private init; }

    public static TimeRange Create(TimeOnly start, TimeOnly end)
    {
        if (start > end)
        {
            throw new Exception("End must be greater than Start");
        }

        return new TimeRange
        {
            Start = start,
            End = end 
        };
    }

    public bool OverlapsWith(TimeRange other)
    {
        return Start < other.End && other.Start < End;
    }
    
    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
    
    private TimeRange (){}
}