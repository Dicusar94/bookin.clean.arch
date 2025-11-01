using BookingApp.Messaging;
using BookingApp.Messaging.Subscriber;
using BookingApp.Persistence;
using BookingApp.Properties;
using BookingApp.Utils.DbSeeders;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace BookingApp.Utils;

public class ApiFactory : WebApplicationFactory<IWebMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder().Build();
    public TestDatabaseReset TestDatabaseReset { get; private set; } = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Overwrite the DBContext setup
            ConfigureDatabaseContext(services);
            RemoveRabbitMq(services);
        });
    }

    private static void RemoveRabbitMq(IServiceCollection services)
    {
        services.RemoveAll<IConnectionFactory>();
        services.RemoveAll<ModelFactory>();
        services.RemoveAll<IMessageProducer>();
        services.RemoveAll<RabbitMqReceiver>();
        services.RemoveAll<IListener>();
        services.RemoveHostedService<WorkerService>();
    }

    public T GetService<T>() where T : notnull
    {
        var scope = Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<T>();
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