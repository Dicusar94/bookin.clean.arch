using BookingApp.Shared;
using Shouldly;

namespace BookingApp.Tests.Unit.Shared.TimeRanges;

public class CreateTimeRangeShould
{
    [Fact]
    public void Create_with_start_greater_that_end_should_fail()
    {
        // arrange & act
        var action = () => TimeRange.Create(
            start: new TimeOnly(8, 0),
            end: new TimeOnly(7, 0));

        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Create_should_create()
    {
        // arrange 
        var start = new TimeOnly(7, 0);
        var end = new TimeOnly(8, 0);
            
        // act
        var timeRange = TimeRange.Create(
            start: start,
            end: end);

        // assert
        timeRange.Start.ShouldBe(start);
        timeRange.End.ShouldBe(end);
    }
}