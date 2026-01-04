using BookingApp.BookingAggregate;
using BookingApp.Persistence.Interceptors;
using BookingApp.Persistence.Repositories;
using BookingApp.RoomAggregate;
using BookingApp.Shared;
using Microsoft.AspNetCore.Builder;
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
        builder.Services.AddSingleton<PublishDomainEventsInterceptor>();
        
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(
                builder.Configuration.GetConnectionString("Default"),
                npgsql => npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "room-booking")));

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

    public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

        if (context is null) return app;

        var migrations = context.Database.GetPendingMigrations();
        
        if (migrations.Any())
        {
            context.Database.Migrate();
        }
        
        return app;
    }
}
