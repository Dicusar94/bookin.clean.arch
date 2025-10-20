using BookingApp.RoomAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Persistence.Configurations;

public class RoomConfigurations : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.HasMany(r => r.Schedules)
            .WithOne()
            .HasForeignKey(rs => rs.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Schedules)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.Ignore(x => x.DomainEvents);
    }
}