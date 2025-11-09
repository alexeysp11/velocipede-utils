using System.Data.Common;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbContexts;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

/// <summary>
/// Interface for database fixture.
/// </summary>
public interface IDatabaseFixture : IAsyncLifetime
{
    /// <summary>
    /// Database name.
    /// </summary>
    string? DatabaseName { get; }

    /// <summary>
    /// Connection string.
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// The identifier of the container.
    /// </summary>
    string ContainerId { get; }

    /// <summary>
    /// Database type.
    /// </summary>
    DatabaseType DatabaseType { get; }

    /// <summary>
    /// Initialize and get .NET <see cref="DbConnection"/>.
    /// </summary>
    /// <returns>.NET <see cref="DbConnection"/></returns>
    DbConnection GetDbConnection();

    /// <summary>
    /// Get database context that contains test models.
    /// </summary>
    /// <returns>Instance of <see cref="TestDbContext"/>.</returns>
    TestDbContext GetTestDbContext();
}
