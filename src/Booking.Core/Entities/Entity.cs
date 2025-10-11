namespace Booking.Core.Entities;

public abstract class Entity
{
    public Guid Id { get; }

    protected Entity(Guid id)
    {
        Id = id;
    }
    
    protected Entity(){}
}