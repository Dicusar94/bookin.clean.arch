using BookingApp.BookingAggregate.Services;
using BookingApp.Persistence;
using BookingApp.Shared;
using BookingApp.Utils;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace BookingApp.DomainServices;

public class BookingPolicyServiceTests(ApiFactory apiFactory) : IClassFixture<ApiFactory>, IAsyncLifetime
{
    private readonly IBookingPolicyService _sut = apiFactory.GetService<IBookingPolicyService>();
    private readonly IUnitOfWork _unitOfWork = apiFactory.GetService<IUnitOfWork>();

    private readonly Func<Task> _initDatabase = apiFactory.TestDatabaseReset.InitializeAsync;
    private readonly Func<Task> _resetDatabase = apiFactory.TestDatabaseReset.ResetAsync;
    private readonly Func<Task> _seedDatabase = apiFactory.SeedAsync;

    [Fact]
    public async Task EnsureBookingCanBeCreatedAsync_should_be_valid()
    {
        // arrange
        var booking = TestBookingFactory.Create(
            roomId: RoomConstants.Room1Id,
            userId: UserConstants.User2Id,
            date: DateTimeConstants.DateNow.AddDays(1),
            timeRange: TimeRangeConstants.FourAmToFivePm,
            timeProvider: DateTimeConstants.TimeProvider);
        
        // act
        var action = async () => await _sut.EnsureBookingCanBeCreatedAsync(booking, CancellationToken.None);
        
        // assert
        await action.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task EnsureBookingCanBeCreatedAsync_with_schedule_out_of_room_schedule_should_fail()
    {
        // arrange
        var booking = TestBookingFactory.Create(
            roomId: RoomConstants.Room2Id,
            userId: UserConstants.User2Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.FiveToSixPm,
            timeProvider: DateTimeConstants.TimeProvider);
        
        // act
        var action = async () => await _sut.EnsureBookingCanBeCreatedAsync(booking, CancellationToken.None);
        
        // assert
        await action.ShouldThrowAsync<Exception>();
    }
    
    [Fact]
    public async Task EnsureBookingCanBeCreatedAsync_with_overlap_bookings_exceeds_room_capacity_should_fail()
    {
        // arrange
        var anotherBooking = TestBookingFactory.Create(
            roomId: RoomConstants.Room2Id,
            userId: UserConstants.User3Id,
            timeRange: TimeRangeConstants.TenAmToTwelvePm,
            id: Guid.NewGuid());

        await _unitOfWork.Bookings.AddBooking(anotherBooking);
        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
        
        var booking = TestBookingFactory.Create(
            roomId: RoomConstants.Room2Id,
            userId: UserConstants.User2Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.TenAmToTwelvePm,
            timeProvider: DateTimeConstants.TimeProvider,
            id: Guid.NewGuid());
        
        // act
        var action = async () => await _sut.EnsureBookingCanBeCreatedAsync(booking, CancellationToken.None);
        
        // assert
        await action.ShouldThrowAsync<Exception>();
    }

    [Fact]
    public async Task EnsureBookingCanBeCreatedAsync_with_overlap_bookings_of_the_same_user_should_fail()
    {
        // arrange
        var booking = TestBookingFactory.Create(
            roomId: RoomConstants.Room2Id,
            userId: UserConstants.User1Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.TenAmToTwelvePm,
            timeProvider: DateTimeConstants.TimeProvider,
            id: Guid.NewGuid());
        
        // act
        var action = async () => await _sut.EnsureBookingCanBeCreatedAsync(booking, CancellationToken.None);
        
        // assert
        await action.ShouldThrowAsync<Exception>();
    }

    public async Task InitializeAsync()
    {
        await _initDatabase();
    }

    public async Task DisposeAsync()
    {
        await _resetDatabase();
        await _seedDatabase();
    }
}