using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

/// <summary>
/// Base class for testing database connection initialization.
/// </summary>
public abstract class BaseDbConnectionInitializeTests
{
    private readonly VelocipedeDatabaseType _databaseType;
    protected string _connectionString;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    protected BaseDbConnectionInitializeTests(VelocipedeDatabaseType databaseType)
    {
        _databaseType = databaseType;
        _connectionString = string.Empty;
    }

    [Fact]
    public void DatabaseType_ShouldBeEqualToSpecified()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);

        // Act & Assert.
        dbConnection.DatabaseType.Should().Be(_databaseType);
    }

    [Fact]
    public void SetConnectionString_SetEmptyWhenCreatingAndChange_ChangedToSpecified()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType);

        // Act.
        string? initialConnectionString = dbConnection.ConnectionString;
        dbConnection.SetConnectionString(_connectionString);

        // Assert.
        _connectionString.Should().NotBeNullOrEmpty();
        initialConnectionString.Should().BeNullOrEmpty();
        dbConnection.ConnectionString.Should().Be(_connectionString);
    }

    [Fact]
    public void SetConnectionString_SpecifyWhenCreating_EqualsToSpecified()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType, _connectionString);

        // Act & Assert.
        _connectionString.Should().NotBeNullOrEmpty();
        dbConnection.ConnectionString.Should().Be(_connectionString);
    }
}
