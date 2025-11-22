using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.OpenDb"/>.
/// </summary>
public abstract class BaseOpenDbTests : BaseDbConnectionTests
{
    protected BaseOpenDbTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void OpenDb_ConnectionStringFromFixture()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
        dbConnection.IsConnected.Should().BeTrue();
    }

    [Fact]
    public void OpenDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void OpenDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void OpenDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.OpenDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }
}
