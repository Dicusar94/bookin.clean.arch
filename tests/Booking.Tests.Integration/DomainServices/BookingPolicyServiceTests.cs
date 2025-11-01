using BookingApp.BookingAggregate.Services;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using Shouldly;

namespace BookingApp.DomainServices;

public class BookingPolicyServiceTests(ApiFactory apiFactory) : IClassFixture<ApiFactory>, IAsyncLifetime
{
    private readonly IBookingPolicyService _sut = apiFactory.GetService<IBookingPolicyService>();
    private readonly IUnitOfWork _unitOfWork = apiFactory.GetService<IUnitOfWork>();

    private readonly Func<Task> _initDatabase = apiFactory.TestDatabaseReset.InitializeAsync;
    private readonly Func<Task> _resetDatabase = apiFactory.TestDatabaseReset.ResetAsync;

    [Fact]
    public async Task EnsureBookingCanBeCreatedAsync_should_be_valid()
    {
        // arrange
        var booking = TestBookingFactory.Create(
            roomId: RoomConstants.Room1Id,
            userId: UserConstants.User1Id,
            date: DateTimeConstants.DateNow.AddDays(1),
            timeRange: TimeRangeConstants.FourAmToFivePm,
            timeProvider: DateTimeConstants.TimeProvider);
        
        // act
        var action = async () => await _sut.EnsureBookingCanBeCreatedAsync(booking, CancellationToken.None);
        
        // assert
        await action.ShouldNotThrowAsync();
    }

    public async Task InitializeAsync()
    {
        await _initDatabase.Invoke();
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase.Invoke();
    }
}