using Npgsql;
using Respawn;

namespace BookingApp.Utils.DbSeeders;

public class TestDatabaseReset(string connectionString)
{
    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public", "postgres", "library", "room-booking"],
        });
    }

    public async Task ResetAsync()
    {
        await using var conn = new NpgsqlConnection(connectionString);
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
    }
}