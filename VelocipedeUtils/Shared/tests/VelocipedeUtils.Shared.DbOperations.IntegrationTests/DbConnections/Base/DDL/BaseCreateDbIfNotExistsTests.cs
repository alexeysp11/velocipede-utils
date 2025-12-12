using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnectionExtensions.CreateDbIfNotExists(IVelocipedeDbConnection)"/>.
/// </summary>
public abstract class BaseCreateDbIfNotExistsTests : BaseDbConnectionTests
{
    protected BaseCreateDbIfNotExistsTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void CreateDbIfNotExists_ConnectionStringFromFixture()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.CreateDbIfNotExists();

        // Act & Assert.
        act.Should().NotThrow<Exception>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateDbIfNotExists_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDbIfNotExists();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public void CreateDbIfNotExists_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDbIfNotExists();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }
}
