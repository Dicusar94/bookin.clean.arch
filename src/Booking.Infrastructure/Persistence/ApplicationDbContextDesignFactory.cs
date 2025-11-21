using BookingApp.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookingApp.Persistence;

public class ApplicationDbContextDesignFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=booking;Username=myuser;Password=mypassword",
            b => b.MigrationsHistoryTable("__EFMigrationsHistory", "room-booking"));

        return new ApplicationDbContext(optionsBuilder.Options, null);
    }
}