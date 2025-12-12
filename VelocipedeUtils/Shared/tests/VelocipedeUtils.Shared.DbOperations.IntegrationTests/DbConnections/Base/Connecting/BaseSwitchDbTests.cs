using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.SwitchDb(string?, out string)"/>.
/// </summary>
public abstract class BaseSwitchDbTests : BaseDbConnectionTests
{
    protected BaseSwitchDbTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void SwitchDb_DbNameFromFixture_ReconnectedWithSameConnectionString()
    {
        // Arrange.
        string connectionString = string.Empty;
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string? dbName = dbConnection.GetActiveDatabaseName();
        Action act = () => dbConnection.SwitchDb(dbName, out connectionString);

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.ConnectionString.Should().Be(connectionString);
        dbConnection.ConnectionString.Should().Be(_fixture.ConnectionString);
        dbConnection.IsConnected.Should().BeTrue();
    }

    [Fact]
    public void SwitchDb_GuidInsteadOfDbName_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string dbName = Guid.NewGuid().ToString();
        Action act = () => dbConnection.SwitchDb(dbName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>();
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void SwitchDb_NullOrEmptyDbName_ThrowsInvalidOperationException(string? dbName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.SwitchDb(dbName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbNameException>()
            .WithMessage(ErrorMessageConstants.IncorrectDatabaseName);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT DATABASE NAME")]
    [InlineData("-;")]
    [InlineData("`<3;")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-database-name")]
    [InlineData("super-not-existing-database-name")]
    public void SwitchDb_IncorrectDbName_ThrowsVelocipedeDbConnectParamsException(string dbName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.SwitchDb(dbName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>();
        dbConnection.IsConnected.Should().BeFalse();
    }
}
