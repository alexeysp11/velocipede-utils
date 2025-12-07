using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

/// <summary>
/// Base class for testing whether database is connected.
/// </summary>
public abstract class BaseDbConnectionIsConnectedTests
{
    private readonly VelocipedeDatabaseType _databaseType;
    protected string _connectionString;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    protected BaseDbConnectionIsConnectedTests(VelocipedeDatabaseType databaseType)
    {
        _databaseType = databaseType;
        _connectionString = string.Empty;
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsConnected_InstanceCreatedAndNoConnectionString_NotConnected(string connectionString)
    {
        // Arrange.
        IVelocipedeDbConnection connection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType, connectionString);

        // Act & Assert.
        connection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void IsConnected_InstanceCreatedAndConnectionStringSet_NotConnected()
    {
        // Arrange.
        IVelocipedeDbConnection connection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType, _connectionString);

        // Act & Assert.
        connection.IsConnected.Should().BeFalse();
    }
}
