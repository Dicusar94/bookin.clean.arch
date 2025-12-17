using BookingApp.Abstractions;
using BookingApp.BackgroundJobs;
using BookingApp.Messaging;
using BookingApp.Messaging.Subscriber;
using BookingApp.Persistence;
using BookingApp.Properties;
using BookingApp.Utils.DbSeeders;
using BookingApp.Utils.Stubs;
using BookingApp.Utils.TestContants.Schared;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Time.Testing;
using Testcontainers.PostgreSql;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Models.Ticker;

namespace BookingApp.Utils;
public class ApiFactory : WebApplicationFactory<IWebMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
    public FakeTimeProvider TimeProvider { get; private set; } = DateTimeConstants.FakeProvider;
    public TestDatabaseReset TestDatabaseReset { get; private set; } = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IHostedService>();
            
            ConfigureDatabaseContext(services);
            RemoveRabbitMq(services);
            RemoveTickerQ(services);
            StubTimeprovider(services);
        });
    }

    private void StubTimeprovider(IServiceCollection services)
    {
        services.RemoveAll<TimeProvider>();
        services.AddSingleton<TimeProvider>(TimeProvider);
    }

    public void ResetTimeProvider()
    {
        TimeProvider = DateTimeConstants.FakeProvider;
    }

    private static void RemoveRabbitMq(IServiceCollection services)
    {
        services.RemoveAll<IConnectionFactory>();
        services.RemoveAll<ModelFactory>();
        services.RemoveAll<IMessageProducer>();
        services.RemoveAll<RabbitMqReceiver>();

        services.AddSingleton<IMessageProducer, InMemorySyncMessageProducer>();
    }

    private static void RemoveTickerQ(IServiceCollection services)
    {
        services.RemoveAll(typeof(ITimeTickerManager<>));
        services.RemoveAll<IHostedService>();
        services.AddSingleton(typeof(ITimeTickerManager<>), typeof(FakeTickerManager<>));
        
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(ITickerFunctionMarker)) // your App layer
            .AddClasses(c => c.AssignableTo<ITickerFunctionMarker>())
            .AsSelf()
            .WithTransientLifetime());
    }

    public T GetService<T>() where T : notnull
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
    }
    
    public FakeTickerManager<T> GetTickerService<T>() where T : TimeTicker
    {
        var result = GetService<ITimeTickerManager<T>>() as FakeTickerManager<T>;
        result?.Clear();
        return result!;
    }

    private void ConfigureDatabaseContext(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType ==
                 typeof(DbContextOptions<ApplicationDbContext>));
            
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
            
        services.AddDbContext<ApplicationDbContext>(opts => 
            opts.UseNpgsql(_postgreSqlContainer.GetConnectionString()));
    }
    
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        Environment.SetEnvironmentVariable("ExternalConfigs__Enabled", "false");
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();
        
        var dbContext = GetService<ApplicationDbContext>();
        
        await dbContext.Database.MigrateAsync();
        await dbContext.SeedAsync();

        TestDatabaseReset = new TestDatabaseReset(_postgreSqlContainer.GetConnectionString());
    }

    public Task SeedAsync()
    {
        var dbContext = GetService<ApplicationDbContext>();
        return dbContext.SeedAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }
}

file static class Extensions
{
    public static void RemoveHostedService<T>(this IServiceCollection services)
        where T : class, IHostedService
    {
        var descriptor = services.SingleOrDefault(
            d => d.ServiceType == typeof(IHostedService) &&
                 d.ImplementationType == typeof(T));

        if (descriptor is not null)
            services.Remove(descriptor);
    }
}
