using BookingApp.BookingAggregate;
using BookingApp.Persistence;
using BookingApp.Utils.TestContants.Bookings;
using BookingApp.Utils.TestContants.Rooms;
using BookingApp.Utils.TestContants.Schared;
using BookingApp.Utils.TestContants.Users;

namespace BookingApp.Utils.DbSeeders;

public static class TestSeedingExtensions
{
    public static async Task SeedAsync(this ApplicationDbContext dbContext, CancellationToken ct = default)
    {
        await dbContext.SeedRoomsAsync(ct);
        await dbContext.SeedBookingAsync(ct);
        await dbContext.SaveChangesAsync(ct);
    }

    private static async Task SeedRoomsAsync(this ApplicationDbContext dbContext, CancellationToken ct = default)
    {
        var recurringSchedule = TestRoomFactory.CreateRecurringSchedule(
            roomId: RoomConstants.Room1Id,
            timeRange: TimeRangeConstants.EightAmToFivePm,
            id: RoomScheduleConstants.Room1Id1Schedule);

        var concreteSchedule = TestRoomFactory.CreateConcreteSchedule(
            roomId: RoomConstants.Room2Id,
            timeRange: TimeRangeConstants.EightAmToFivePm,
            id: RoomScheduleConstants.Room2Id1Schedule);

        var room1 = TestRoomFactory.CreateRoom(
            id: RoomConstants.Room1Id,
            name: "Room1Recurring",
            capacity: 2);
            
            room1.Activate(DateTimeConstants.TimeProvider);
        
        var room2 = TestRoomFactory.CreateRoom(
            id: RoomConstants.Room2Id,
            name: "Room2Concrete",
            capacity: 2);
        
            room2.Activate(DateTimeConstants.TimeProvider);
        
        room1.AddSchedule(recurringSchedule);
        room2.AddSchedule(concreteSchedule);

        await dbContext.Rooms.AddRangeAsync([room1, room2], ct);
    }

    private static async Task SeedBookingAsync(this ApplicationDbContext dbContext, CancellationToken ct = default)
    {
        var bookingRoom1 = TestBookingFactory.Create(
            roomId: RoomConstants.Room1Id,
            userId: UserConstants.User1Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.NineAmToElevenAm,
            timeProvider: DateTimeConstants.TimeProvider,
            id: BookingConstants.Booking1Id);
        
        var bookingRoom2 = TestBookingFactory.Create(
            roomId: RoomConstants.Room2Id,
            userId: UserConstants.User1Id,
            date: DateTimeConstants.DateNow,
            timeRange: TimeRangeConstants.NineAmToElevenAm,
            timeProvider: DateTimeConstants.TimeProvider,
            id: BookingConstants.Booking2Id);

        await dbContext.Bookings.AddRangeAsync([bookingRoom1, bookingRoom2], ct);
    }
}