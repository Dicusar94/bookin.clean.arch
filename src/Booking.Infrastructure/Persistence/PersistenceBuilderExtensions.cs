using BookingApp.BookingAggregate;
using BookingApp.Persistence.Repositories;
using BookingApp.RoomAggregate;
using BookingApp.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.DependencyInjection;

namespace BookingApp.Persistence;

public static class PersistenceBuilderExtensions
{
    internal static IHostApplicationBuilder AddPersistence(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(opt => 
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

        builder.AddRepositories();
        builder.AddBackgroundJobs();

        return builder;
    }

    private static IHostApplicationBuilder AddRepositories(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();

        return builder;
    }

    private static IHostApplicationBuilder AddBackgroundJobs(this IHostApplicationBuilder builder)
    {
        builder.Services.AddTickerQ(options =>
        {
            options.SetMaxConcurrency(10);
            
            options.AddOperationalStore<ApplicationDbContext>(efOptions =>
            {
                efOptions.UseModelCustomizerForMigrations();
            });

            options.AddDashboard(uiopt =>
            {
                uiopt.BasePath = "/tickerq-dashboard";
                uiopt.EnableBasicAuth = false;
            });
        });

        return builder;
    }
}