using Ardalis.GuardClauses;
using Booking.Core.Entities;
using Booking.Domain.RoomAggregate.Events;

namespace Booking.Domain.RoomAggregate;

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
    
    private Room(){}
}