using BookingApp.RoomAggregate.Events;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using Shouldly;

namespace BookingApp.Tests.Unit.Rooms;

public class RescheduleRoomShould
{
    [Fact]
    public void Reschedule_should_reschedule()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var schedule = TestRoomFactory.CreateRecurringSchedule(
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        room.AddSchedule(schedule);

        var newTimeRange = TimeRangeConstants.ElevenAmToTwelvePm;

        // act
        room.Reschedule(roomScheduleId: schedule.Id, newTimeRange);

        // assert
        schedule.TimeRange.ShouldBe(newTimeRange);
        room.Schedules.Count.ShouldBe(1);
        room.DomainEvents.OfType<RoomScheduleChangedEvent>().Count().ShouldBe(2);
    }
    
    [Fact]
    public void Reschedule_with_overlap_timerange_should_fail()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var schedule1 = TestRoomFactory.CreateRecurringSchedule(
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        var schedule2 = TestRoomFactory.CreateRecurringSchedule(
            timeRange: TimeRangeConstants.ElevenAmToTwelvePm);
        
        room.AddSchedule(schedule1);
        room.AddSchedule(schedule2);

        var newTimeRange = TimeRangeConstants.TenAmToTwelvePm;

        // act
        var action = () => room.Reschedule(roomScheduleId: schedule1.Id, newTimeRange);

        // assert
        action.ShouldThrow<Exception>();
    }
    
    [Fact]
    public void Reschedule_with_non_existing_schedule_should_fail()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var schedule1 = TestRoomFactory.CreateRecurringSchedule(
            timeRange: TimeRangeConstants.NineAmToElevenAm);
        
        var schedule2 = TestRoomFactory.CreateRecurringSchedule(
            timeRange: TimeRangeConstants.ElevenAmToTwelvePm);
        
        room.AddSchedule(schedule1);

        var newTimeRange = TimeRangeConstants.TenAmToTwelvePm;

        // act
        var action = () => room.Reschedule(roomScheduleId: schedule2.Id, newTimeRange);

        // assert
        action.ShouldThrow<Exception>();
    }
}