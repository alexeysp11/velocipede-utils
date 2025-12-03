using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.CreateDb"/>.
/// </summary>
public abstract class BaseCreateDbTests : BaseDbConnectionTests
{
    protected BaseCreateDbTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public virtual void CreateDb_ConnectAndSetNotExistingDbUsingSetters()
    {
        // Arrange.
        string dbName = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        if (_fixture.DatabaseType == DatabaseType.SQLite)
            dbName = $"{dbName}.db";

        // Act.
        dbConnection
            .OpenDb()
            .GetConnectionString(dbName, out string newConnectionString)
            .SetConnectionString(newConnectionString)
            .CreateDb()
            .SwitchDb(dbName)
            .CloseDb();
        bool dbExists = dbConnection.DbExists();

        // Assert.
        dbExists.Should().BeTrue();
    }

    [Fact]
    public virtual void CreateDb_CreateNotExistingDbUsingExtensionMethod()
    {
        // Arrange.
        string dbName = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .CreateDb(dbName)
            .SwitchDb(dbName)
            .CloseDb();
        bool dbExists = dbConnection.DbExists();

        // Assert.
        dbExists.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void CreateDb_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDb();

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
    public void CreateDb_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public void CreateDb_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
    }

    [Fact]
    public void CreateDb_ConnectionStringFromFixture_ThrowsInvalidOperationException()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Action act = () => dbConnection.CreateDb();

        // Act & Assert.
        act
            .Should()
            .Throw<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.DatabaseAlreadyExists);
    }
}
