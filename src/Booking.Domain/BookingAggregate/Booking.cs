using BookingApp.BookingAggregate.Events;
using BookingApp.Entities;
using BookingApp.Shared;

namespace BookingApp.BookingAggregate;

public class Booking : AggregateRoot
{
    private static readonly TimeSpan minDuration = TimeSpan.FromMinutes(30);
    private static readonly TimeSpan maxDuration = TimeSpan.FromHours(8);
    
    public static TimeSpan MaxPendingStatusDuration = TimeSpan.FromMinutes(15);
    
    public Guid RoomId { get; private set; }
    public Guid UserId { get; private set; }
    public DateOnly Date  { get; private set; }
    public TimeRange TimeRange { get; private set; } = null!;
    public BookingStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }
    public DateTime? CanceledAt { get; private set; }

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
        CreatedAt = timeProvider.GetUtcNow().UtcDateTime;
        
        AddDomainEvent(new BookingCreatedEvent(Id));
        AddDomainEvent(new BookingPendingConfirmationEvent(Id, timeProvider.GetUtcNow()));
    }

    public void Confirm(TimeProvider timeProvider)
    {
        if (Status != BookingStatus.Pending)
        {
            throw new Exception("Only pending booking can be confirmed");
        }

        if (CreatedAt.Add(MaxPendingStatusDuration) >= timeProvider.GetUtcNow().UtcDateTime)
        {
            throw new Exception("Can't confirm booking, pending time has expired.");
        }

        Status = BookingStatus.Confirmed;
        ConfirmedAt = timeProvider.GetUtcNow().UtcDateTime;
        AddDomainEvent(new BookingConfirmedEvent(Id));
    }

    public void AutoCancel(TimeProvider timeProvider)
    {
        if (Status != BookingStatus.Pending)
        {
            throw new Exception("Only pending booking can be auto-canceled");
        }

        var durationInPending = timeProvider.GetUtcNow().UtcDateTime - CreatedAt;
        
        if (durationInPending < MaxPendingStatusDuration)
        {
            throw new Exception("Max duration in pending state didn't exceed");
        }
        
        Status = BookingStatus.Canceled;
        CanceledAt = timeProvider.GetUtcNow().UtcDateTime;
        AddDomainEvent(new BookingAutoCanceledEvent(Id));
    }

    public void CancelConfirmed(TimeProvider timeProvider)
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new Exception("Booking already canceled");
        }

        Status = BookingStatus.Canceled;
        CanceledAt = timeProvider.GetUtcNow().UtcDateTime;
        AddDomainEvent(new BookingCanceledConfirmedEvent(Id));
    }
    
    private Booking(){}
}