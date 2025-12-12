using BookingApp.Exceptions;
using BookingApp.RoomAggregate;
using BookingApp.Telemetry;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Persistence.Repositories;

public class RoomRepository(ApplicationDbContext context) : IRoomRepository
{
    public async Task<Room> AddRoom(Room room, CancellationToken ct)
    {
        var dbActivity = RunTimeDiagnosticConfig.Source.StartActivity()
            .SetRoomId(room.Id)
            .SetRoomName(room.Name)
            .SetRoomStatus(room.Status.ToString());
        
        if (context.Rooms.Any(x => x.Name == room.Name))
        {
            var exception = new EntityAlreadyExistsException($"Room with name {room.Name} already exists.");
            dbActivity.AddExceptionAndFail(exception);
            throw exception;
        }

        await context.Rooms.AddAsync(room, ct);
        return room;
    }

    public Task<Room> UpdateRoom(Room room, CancellationToken _)
    {
        var dbActivity = RunTimeDiagnosticConfig.Source.StartActivity()
            .SetRoomId(room.Id)
            .SetRoomName(room.Name)
            .SetRoomStatus(room.Status.ToString());
        
        if (context.Rooms.Any(x => x.Name == room.Name && x.Id != room.Id))
        {
            var exception = new EntityAlreadyExistsException($"Room with name {room.Name} already exists.");
            dbActivity.AddExceptionAndFail(exception);
            throw exception;
        }

        context.Rooms.Update(room);
        return Task.FromResult(room);
    }

    public async Task<Room> GetRoomById(Guid id, CancellationToken ct)
    {
        var dbActivity = RunTimeDiagnosticConfig.Source.StartActivity()
            .SetRoomId(id);

        var room = await context.Rooms
            .Include(x => x.Schedules)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (room is null)
        {
            var exception = new EntityNotFoundException(nameof(Room), id.ToString());
            dbActivity.AddExceptionAndFail(exception);
            throw exception;
        }

        return room;
    }

    public Task<List<Room>> GetRooms(CancellationToken ct)
    {
        return context.Rooms.ToListAsync(ct);
    }
}