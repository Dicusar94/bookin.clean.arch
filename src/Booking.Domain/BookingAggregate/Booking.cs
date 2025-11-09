using BookingApp.BookingAggregate.Events;
using BookingApp.Entities;
using BookingApp.Shared;

namespace BookingApp.BookingAggregate;

public class Booking : AggregateRoot
{
    private static readonly TimeSpan minDuration = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan maxDuration = TimeSpan.FromHours(8);
    public Guid RoomId { get; private set; }
    public Guid UserId { get; private set; }
    public DateOnly Date  { get; private set; }
    public TimeRange TimeRange { get; private set; } = null!;
    public BookingStatus Status { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CanceledAt { get; set; }

    public Booking(
        Guid roomId,
        Guid userId,
        DateOnly date,
        TimeRange timeRange,
        TimeProvider timeProvider,
        Guid? id = null) : base(id ?? Guid.NewGuid())
    {
        if (!timeRange.HasValidDuration(minDuration, maxDuration))
        {
            var message =
                $"Booking can't less than {minDuration.TotalMinutes}min " +
                $"and more than {minDuration.TotalHours}hours";
                    
            throw new ApplicationException(message);
        }

        if (timeProvider.GetUtcNow() > new DateTime(date, timeRange.Start))
        {
            throw new ApplicationException("Booking can be only in the future");
        }
        
        RoomId = roomId;
        UserId = userId;
        Date = date;
        TimeRange = timeRange;
        Status = BookingStatus.Pending;
        CreatedAt = timeProvider.GetUtcNow().DateTime;
        
        AddDomainEvent(new BookingCreatedEvent(Id));
        AddDomainEvent(new BookingPendingConfirmationEvent(Id, timeProvider.GetUtcNow()));
    }

    public void Confirm(TimeProvider timeProvider)
    {
        if (Status != BookingStatus.Pending)
        {
            throw new Exception("Only pending booking can be confirmed");
        }

        Status = BookingStatus.Confirmed;
        ConfirmedAt = timeProvider.GetUtcNow().DateTime;
        AddDomainEvent(new BookingConfirmedEvent(Id));
    }

    public void AutoCancel(TimeProvider timeProvider)
    {
        if (Status != BookingStatus.Pending)
        {
            throw new Exception("Only pending booking can be auto-canceled");
        }
        
        Status = BookingStatus.Canceled;
        CanceledAt = timeProvider.GetUtcNow().DateTime;
        AddDomainEvent(new BookingConfirmedEvent(Id));
    }
    
    private Booking(){}
}