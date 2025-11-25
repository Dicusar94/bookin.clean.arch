using Microsoft.Extensions.Time.Testing;

namespace BookingApp.Utils;

public abstract class BaseAsyncLifeTime(ApiFactory apiFactory) : IAsyncLifetime
{
    protected virtual Func<Task> _initDatabase { get; } = apiFactory.TestDatabaseReset.InitializeAsync;
    protected virtual Func<Task> _resetDatabase { get; } = apiFactory.TestDatabaseReset.ResetAsync;
    protected virtual Func<Task> _seedDatabase { get; } = apiFactory.SeedAsync;
    protected virtual Action _resetTimeProvider { get; } = apiFactory.ResetTimeProvider;
    protected FakeTimeProvider TimeProvider => apiFactory.TimeProvider;
    
    public Task InitializeAsync()
    {
        return _initDatabase();
    }

    public async Task DisposeAsync()
    {
        _resetTimeProvider.Invoke();
        await _resetDatabase();
        await _seedDatabase();
    }
}