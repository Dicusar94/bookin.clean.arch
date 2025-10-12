using System.Xml.Schema;
using Ardalis.GuardClauses;
using Booking.Core.Entities;

namespace Booking.Domain.RoomAggregate;

public class Room : AggregateRoot
{
    private readonly List<RoomSchedule> _schedules = [];
    
    public string Name { get; set; } = null!;
    public int Capacity { get; set; }
    public RoomStatus Status { get; set; }
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
    
    private bool HasOverlapWithExisting(RoomSchedule newSchedule)
        => _schedules.Any(existing => existing.OverlapsWith(newSchedule)); 
    
    private Room(){}
}