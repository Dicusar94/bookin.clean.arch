using BookingApp.BookingAggregate.Services;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using Shouldly;

namespace BookingApp.DomainServices;

public class BookingPolicyServiceTests : IClassFixture<ApiFactory>, IAsyncLifetime
{
    private readonly IBookingPolicyService _sut;
    private readonly IUnitOfWork _unitOfWork;

    private Func<Task> _initDatabase;
    private Func<Task> _resetDatabase;
    
    public BookingPolicyServiceTests(ApiFactory apiFactory)
    {
        _sut = apiFactory.GetService<IBookingPolicyService>();
        _unitOfWork = apiFactory.GetService<IUnitOfWork>();
        _initDatabase = apiFactory.TestDatabaseReset.InitializeAsync;
        _resetDatabase = apiFactory.TestDatabaseReset.ResetAsync;

    }

    [Fact]
    public async Task EnsureBookingCanBeCreatedAsync_should_be_valid()
    {
        // arrange
        var room = TestRoomFactory.CreateRoom();
        
        var schedule = TestRoomFactory.CreateRecurringSchedule(
            roomId: room.Id,
            dayOfWeek: DateTimeConstants.DayOfWeek,
            timeRange: TimeRangeConstants.EightAmToFivePm);
        
        room.AddSchedule(schedule);
        await _unitOfWork.Rooms.AddRoom(room, CancellationToken.None);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        var booking = TestBookingFactory.Create(
            roomId: room.Id,
            userId: UserConstants.User1Id,
            date: DateTimeConstants.DateNow,
            timeProvider: DateTimeConstants.TimeProvider);
        
        // act
        var action = async () => await _sut.EnsureBookingCanBeCreatedAsync(booking, CancellationToken.None);
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