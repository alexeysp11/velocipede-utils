using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.CloseDb"/>.
/// </summary>
public abstract class BaseCloseDbTests : BaseDbConnectionTests
{
    protected BaseCloseDbTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void CloseDb_ConnectionStringFromFixture()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void CloseDb_FixtureConnected()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.OpenDb();
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public void CloseDb_GuidInsteadOfConnectionString()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CloseDb_NullOrEmptyConnectionString(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void CloseDb_IncorrectConnectionString(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CloseDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeFalse();
    }
}
