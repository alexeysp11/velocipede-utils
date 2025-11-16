using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;

public abstract class BaseDbExistsTests : BaseDbConnectionTests
{
    protected BaseDbExistsTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void DbExists_ConnectionStringFromFixture_ReturnsTrue()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        bool result = dbConnection.DbExists();

        // Assert.
        result.Should().BeTrue();
    }

    [Fact]
    public void DbExists_ConnectAndSetNotExistingDb_ReturnsFalse()
    {
        // Arrange.
        string dbName = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection
            .GetConnectionString(dbName, out string newConnectionString)
            .SetConnectionString(newConnectionString);

        // Act.
        bool result = dbConnection.DbExists();

        // Assert.
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void DbExists_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.DbExists();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Fact]
    public void DbExists_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.DbExists();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void DbExists_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.DbExists();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }
}
