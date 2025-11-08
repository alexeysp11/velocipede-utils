using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Testcontainers.PostgreSql;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

public class PgDatabaseFixture : IDatabaseFixture
{
    private readonly PostgreSqlContainer container =
        new PostgreSqlBuilder()
            .WithEnvironment("POSTGRES_HOST_AUTH_METHOD", "password")
            .Build();

    /// <inheritdoc/>
    public string? DatabaseName => PgDbConnection.GetDatabaseName(ConnectionString);

    /// <inheritdoc/>
    public string ConnectionString => container.GetConnectionString();

    /// <inheritdoc/>
    public string ContainerId => $"{container.Id}";

    /// <inheritdoc/>
    public DatabaseType DatabaseType => DatabaseType.PostgreSQL;

    /// <inheritdoc/>
    public DbConnection GetDbConnection()
        => new NpgsqlConnection(ConnectionString);

    /// <inheritdoc/>
    public TestDbContext GetTestDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseNpgsql(ConnectionString);

        return new TestDbContext(optionsBuilder.Options);
    }

    /// <inheritdoc/>
    public Task InitializeAsync()
        => container.StartAsync();

    /// <inheritdoc/>
    public Task DisposeAsync()
        => container.DisposeAsync().AsTask();
}
