using Ardalis.GuardClauses;
using BookingApp.Entities;
using BookingApp.RoomAggregate.Events;
using BookingApp.Shared;

namespace BookingApp.RoomAggregate;

public class Room : AggregateRoot
{
    private readonly List<RoomSchedule> _schedules = [];
    
    public string Name { get; private set; } = null!;
    public int Capacity { get; private set; }
    public RoomStatus Status { get; private set; }
    public IReadOnlyList<RoomSchedule> Schedules => _schedules.AsReadOnly();

    public Room(string name, int capacity, Guid? id = null) 
        : base(id ?? Guid.NewGuid())
    {
        Guard.Against.NullOrWhiteSpace(name);
        Guard.Against.NegativeOrZero(capacity);

        Name = name;
        Capacity = capacity;
        Status = RoomStatus.Inactive;
    }

    public void AddSchedule(RoomSchedule schedule)
    {
        if (Id != schedule.RoomId)
        {
            throw new Exception("Cannot add schedule from other room");
        }
        
        if (HasOverlapWithExisting(schedule))
        {
            throw new Exception($"Cannot add schedule on {schedule} — it overlaps with an existing one.");
        }
        
        _schedules.Add(schedule);
        
        AddDomainEvent(new RoomScheduleChangedEvent(
            Id: Id,
            RoomScheduleId: schedule.Id));
    }

    public RoomSchedule Reschedule(Guid roomScheduleId, TimeRange newTimeRange)
    {
        var schedule = Schedules.FirstOrDefault(x => x.Id == roomScheduleId);

        if (schedule is null)
        {
            throw new Exception("Schedule not found.");
        }

        if (HasOverlapWithExisting(roomScheduleId, schedule.WithTimeRange(newTimeRange)))
        {
            throw new Exception($"Cannot reschedule on {schedule} — it overlaps with an existing one.");
        }
        
        schedule.RescheduleTimeRange(newTimeRange);
        
        AddDomainEvent(new RoomScheduleChangedEvent(
            Id: Id,
            RoomScheduleId: roomScheduleId));

        return schedule;
    }

    public void Activate(TimeProvider timeProvider)
    {
        Guard.Against.InvalidInput(
            input:Status, 
            parameterName: nameof(Status), 
            predicate: status => status is RoomStatus.Inactive, 
            message:"Room already active");

        if (!Schedules.Any())
        {
            throw new Exception("Room has not set any schedules");
        }
        
        if (!Schedules.Any(x => x.IsActive(timeProvider)))
        {
            throw new Exception("Room has no active schedule");
        }
        
        Status = RoomStatus.Active;
        AddDomainEvent(new RoomActivatedEvent(Id, timeProvider.GetUtcNow().UtcDateTime));
    }

    public void Deactivate(TimeProvider timeProvider)
    {
        Guard.Against.InvalidInput(
            input:Status, 
            parameterName: nameof(Status), 
            predicate: status => status is RoomStatus.Active, 
            message:"Room already inactive");
        
        Status = RoomStatus.Inactive;
        AddDomainEvent(new RoomDeactivatedEvent(Id, timeProvider.GetUtcNow().UtcDateTime));
    }

    public bool IsAvailableOn(DateOnly date, TimeRange requestedTime)
    {
        return _schedules.Any(x => x.CoversWith(date, requestedTime));
    }
    
    private bool HasOverlapWithExisting(RoomSchedule newSchedule)
        => _schedules.Any(existing => existing.OverlapsWith(newSchedule)); 
    
    private bool HasOverlapWithExisting(Guid roomScheduleId, RoomSchedule newSchedule)
        => _schedules.Any(existing => 
            existing.Id != roomScheduleId && 
            existing.OverlapsWith(newSchedule)); 
    
    private Room(){}
}