namespace Booking.Core.Entities;

public abstract class AggregateRoot : Entity
{
    protected AggregateRoot(Guid id) : base(id){}
    protected AggregateRoot() {}
}