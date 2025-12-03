using System.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

/// <summary>
/// MS SQL fixture.
/// </summary>
public sealed class MssqlDatabaseFixture : IDatabaseFixture
{
    private readonly MsSqlContainer container;

    /// <inheritdoc/>
    public string? DatabaseName => MssqlDbConnection.GetDatabaseName(ConnectionString);

    /// <inheritdoc/>
    public string ConnectionString => container.GetConnectionString()
        .Replace("Server=", "Data Source=")
        .Replace("Database", "Initial Catalog")
        .Replace("User Id", "User ID")
        .Replace("TrustServerCertificate", "Trust Server Certificate");

    /// <inheritdoc/>
    public string ContainerId => $"{container.Id}";

    /// <inheritdoc/>
    public DatabaseType DatabaseType => DatabaseType.MSSQL;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public MssqlDatabaseFixture()
    {
        container = new MsSqlBuilder().Build();
    }

    /// <inheritdoc/>
    public DbConnection GetDbConnection()
        => new SqlConnection(ConnectionString);

    /// <inheritdoc/>
    public TestDbContext GetTestDbContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        optionsBuilder.UseSqlServer(ConnectionString);

        return new TestDbContext(optionsBuilder.Options);
    }

    /// <inheritdoc/>
    public Task InitializeAsync()
        => container.StartAsync();

    /// <inheritdoc/>
    public Task DisposeAsync()
        => container.DisposeAsync().AsTask();
}
