using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

/// <summary>
/// SQLite fixture.
/// </summary>
public class SqliteDatabaseFixture : IDatabaseFixture
{
    /// <inheritdoc/>
    public string? DatabaseName => SqliteDbConnection.GetDatabaseName(ConnectionString);

    /// <inheritdoc/>
    public string ConnectionString { get; private set; }

    /// <inheritdoc/>
    public string ContainerId { get; private set; }

    /// <inheritdoc/>
    public DatabaseType DatabaseType => DatabaseType.SQLite;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqliteDatabaseFixture()
    {
        ContainerId = Guid.NewGuid().ToString();
        ConnectionString = SqliteDbConnection.GetConnectionString($"{ContainerId}.db");
    }

    /// <inheritdoc/>
    public DbConnection GetDbConnection()
        => new SqliteConnection(ConnectionString);

    /// <inheritdoc/>
    public TestDbContext GetTestDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseSqlite(ConnectionString);

        return new TestDbContext(optionsBuilder.Options);
    }

    /// <inheritdoc/>
    public Task InitializeAsync()
    {
        if (string.IsNullOrEmpty(DatabaseName))
            throw new ArgumentNullException(nameof(DatabaseName));

        if (!File.Exists(DatabaseName))
        {
            File.Create(DatabaseName).Close();
        }
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task DisposeAsync()
    {
        if (File.Exists(DatabaseName))
        {
            SqliteConnection.ClearAllPools();
            File.Delete(DatabaseName);
        }
        return Task.CompletedTask;
    }
}
