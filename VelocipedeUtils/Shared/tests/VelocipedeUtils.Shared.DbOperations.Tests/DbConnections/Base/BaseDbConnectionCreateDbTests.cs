using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Factories;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

/// <summary>
/// Base class for testing create database operation.
/// </summary>
public abstract class BaseDbConnectionCreateDbTests
{
    private readonly DatabaseType _databaseType;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="databaseType">Database type.</param>
    protected BaseDbConnectionCreateDbTests(DatabaseType databaseType)
    {
        _databaseType = databaseType;
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void DbCreate_ConnectionStringIsNullOrEmpty_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = VelocipedeDbConnectionFactory.InitializeDbConnection(_databaseType, connectionString);
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act.Should().Throw<InvalidOperationException>();
    }
}
