using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

/// <summary>
/// Resolver for multiple database fixtures.
/// </summary>
public sealed class DatabaseFixtureResolver : IAsyncLifetime
{
    private readonly Dictionary<VelocipedeDatabaseType, IDatabaseFixture> _databaseFixtures;

    public DatabaseFixtureResolver()
    {
        PgDatabaseFixture pgFixture = new();
        SqlServerDatabaseFixture sqlServerFixture = new();
        SqliteDatabaseFixture sqliteFixture = new();

        _databaseFixtures = new Dictionary<VelocipedeDatabaseType, IDatabaseFixture>
        {
            { VelocipedeDatabaseType.PostgreSQL, pgFixture },
            { VelocipedeDatabaseType.MSSQL, sqlServerFixture },
            { VelocipedeDatabaseType.SQLite, sqliteFixture }
        };
    }

    public async Task InitializeAsync()
    {
        await _databaseFixtures[VelocipedeDatabaseType.PostgreSQL].InitializeAsync();
        await _databaseFixtures[VelocipedeDatabaseType.MSSQL].InitializeAsync();
        await _databaseFixtures[VelocipedeDatabaseType.SQLite].InitializeAsync();
    }

    public async Task DisposeAsync()
    {
        await _databaseFixtures[VelocipedeDatabaseType.PostgreSQL].DisposeAsync();
        await _databaseFixtures[VelocipedeDatabaseType.MSSQL].DisposeAsync();
        await _databaseFixtures[VelocipedeDatabaseType.SQLite].DisposeAsync();
    }

    public IDatabaseFixture GetFixture(VelocipedeDatabaseType dbType)
    {
        if (_databaseFixtures.TryGetValue(dbType, out IDatabaseFixture? fixture))
        {
            return fixture ?? throw new InvalidCastException($"Fixture for {dbType} is not of expected type {typeof(IDatabaseFixture).Name}");
        }
        throw new KeyNotFoundException($"No fixture found for database type: {dbType}");
    }
}
