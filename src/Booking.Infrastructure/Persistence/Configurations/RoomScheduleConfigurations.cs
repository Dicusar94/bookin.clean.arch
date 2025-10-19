using BookingApp.RoomAggregate;
using BookingApp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApp.Persistence.Configurations;

public class RoomScheduleConfigurations : IEntityTypeConfiguration<RoomSchedule>
{
    public void Configure(EntityTypeBuilder<RoomSchedule> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Date)
            .HasColumnType("date")
            .IsRequired(false);
        
        builder.Property(x => x.DayOfWeek);

        builder.Property(x => x.IsRecurring);
        
        builder.Property(x => x.RoomId);

        builder.OwnsOne(x => x.TimeRange, navigationBuilder =>
        {
            navigationBuilder.Property(x => x.Start)
                .HasColumnType("time")
                .IsRequired();
            
            navigationBuilder.Property(x => x.End)
                .HasColumnType("time")
                .IsRequired();
        });
        
        // optional: composite index for recurring schedules
        builder.HasIndex(rs => new { rs.RoomId, rs.DayOfWeek, rs.IsRecurring, rs.Date }); 
    }
}