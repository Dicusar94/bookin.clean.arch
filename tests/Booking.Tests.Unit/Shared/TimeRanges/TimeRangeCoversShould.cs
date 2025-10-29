using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Shared.TimeRanges;

public class TimeRangeCoversShould
{
    [Fact]
    public void CoversWith_should_be_true()
    {
        // arrange
        var timeRange = TimeRangeConstants.EightAmToFivePm;

        // act
        var result = timeRange.CoversWith(TimeRangeConstants.FourAmToFivePm);
        
        // assert
        result.ShouldBeTrue();
    }
    
    [Fact]
    public void CoversWith_should_be_false()
    {
        // arrange
        var timeRange = TimeRangeConstants.NineAmToElevenAm;

        // act
        var result = timeRange.CoversWith(TimeRangeConstants.TenAmToTwelvePm);
        
        // assert
        result.ShouldBeFalse();
    }
}