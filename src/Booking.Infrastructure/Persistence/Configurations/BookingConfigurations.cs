using BookingApp.BookingAggregate;
using BookingApp.RoomAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Persistence.Configurations;

public class BookingConfigurations : IEntityTypeConfiguration<BookingAggregate.Booking>
{
    public void Configure(EntityTypeBuilder<BookingAggregate.Booking> builder)
    {
        builder.ToTable("Bookings");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Date)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();
        
        builder.OwnsOne(x => x.TimeRange, navigationBuilder =>
        {
            navigationBuilder.Property(x => x.Start)
                .HasColumnType("time")
                .IsRequired();
            
            navigationBuilder.Property(x => x.End)
                .HasColumnType("time")
                .IsRequired();
        });

        builder.Ignore(x => x.DomainEvents);
    }
}