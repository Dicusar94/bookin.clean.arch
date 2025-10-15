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

    public void Reschedule(Guid roomScheduleId, TimeRange newTimeRange)
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
    }

    public void Activate()
    {
        Status = RoomStatus.Active;
    }

    public void Deactivate(TimeProvider timeProvider)
    {
        Status = RoomStatus.Inactive;
        
        AddDomainEvent(new RoomDeactivatedEvent(
            Id: Id,
            OnDateTime: timeProvider.GetUtcNow().DateTime));
    }
    
    private bool HasOverlapWithExisting(RoomSchedule newSchedule)
        => _schedules.Any(existing => existing.OverlapsWith(newSchedule)); 
    
    private bool HasOverlapWithExisting(Guid roomScheduleId, RoomSchedule newSchedule)
        => _schedules.Any(existing => 
            existing.Id != roomScheduleId && 
            existing.OverlapsWith(newSchedule)); 
    
    private Room(){}
}