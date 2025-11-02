using BookingApp.Utils;

namespace BookingApp.Applications.RoomSchedules;

[Collection(CollectionConstants.ApplicationTests)]
public class AddConcreteRoomScheduleCommandTests : IAsyncLifetime
{
    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}