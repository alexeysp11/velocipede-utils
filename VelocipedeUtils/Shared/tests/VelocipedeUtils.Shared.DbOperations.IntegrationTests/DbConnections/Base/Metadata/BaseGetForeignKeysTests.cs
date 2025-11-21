using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.GetForeignKeys(string, out List{VelocipedeForeignKeyInfo})"/>.
/// </summary>
public abstract class BaseGetForeignKeysTests : BaseDbConnectionTests
{
    protected BaseGetForeignKeysTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("TestModels", 0)]
    [InlineData("testModels", 0)]
    [InlineData("testmodels", 0)]
    [InlineData("Testmodels", 0)]
    [InlineData("TESTMODELS", 0)]
    [InlineData("\"TestUsers\"", 1)]
    [InlineData("TestUsers", 1)]
    [InlineData("testUsers", 1)]
    [InlineData("testusers", 1)]
    [InlineData("Testusers", 1)]
    [InlineData("TESTUSERS", 1)]
    public void GetForeignKeys_FixtureNotConnected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo>? result);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("TestModels", 0)]
    [InlineData("testModels", 0)]
    [InlineData("testmodels", 0)]
    [InlineData("Testmodels", 0)]
    [InlineData("TESTMODELS", 0)]
    [InlineData("\"TestUsers\"", 1)]
    [InlineData("TestUsers", 1)]
    [InlineData("testUsers", 1)]
    [InlineData("testusers", 1)]
    [InlineData("Testusers", 1)]
    [InlineData("TESTUSERS", 1)]
    public void GetForeignKeys_FixtureConnected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .GetForeignKeys(tableName, out List<VelocipedeForeignKeyInfo>? result)
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("TestModels")]
    [InlineData("testModels")]
    [InlineData("testmodels")]
    [InlineData("Testmodels")]
    [InlineData("TESTMODELS")]
    public void GetForeignKeys_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException(string tableName)
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetForeignKeys(tableName, out _);

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
    public void GetForeignKeys_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetForeignKeys(tableName, out _);

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
    public void GetForeignKeys_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetForeignKeys(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("TestModels", 0)]
    [InlineData("testModels", 0)]
    [InlineData("testmodels", 0)]
    [InlineData("Testmodels", 0)]
    [InlineData("TESTMODELS", 0)]
    [InlineData("\"TestUsers\"", 1)]
    [InlineData("TestUsers", 1)]
    [InlineData("testUsers", 1)]
    [InlineData("testusers", 1)]
    [InlineData("Testusers", 1)]
    [InlineData("TESTUSERS", 1)]
    public async Task GetForeignKeysAsync_FixtureNotConnected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeForeignKeyInfo>? result = await dbConnection.GetForeignKeysAsync(tableName);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("TestModels", 0)]
    [InlineData("testModels", 0)]
    [InlineData("testmodels", 0)]
    [InlineData("Testmodels", 0)]
    [InlineData("TESTMODELS", 0)]
    [InlineData("\"TestUsers\"", 1)]
    [InlineData("TestUsers", 1)]
    [InlineData("testUsers", 1)]
    [InlineData("testusers", 1)]
    [InlineData("Testusers", 1)]
    [InlineData("TESTUSERS", 1)]
    public async Task GetForeignKeysAsync_FixtureConnected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeForeignKeyInfo>? result = await dbConnection
            .OpenDb()
            .GetForeignKeysAsync(tableName);
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("TestModels")]
    [InlineData("testModels")]
    [InlineData("testmodels")]
    [InlineData("Testmodels")]
    [InlineData("TESTMODELS")]
    public async Task GetForeignKeysAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException(string tableName)
    {
        // Arrange.
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeForeignKeyInfo>>> act = async () => await dbConnection.GetForeignKeysAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task GetForeignKeysAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<object>> act = async () => await dbConnection.GetForeignKeysAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage(ErrorMessageConstants.ConnectionStringShouldNotBeNullOrEmpty);
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("INCORRECT CONNECTION STRING")]
    [InlineData("connect:localhost:0000;")]
    [InlineData("connect:localhost:0000;super-connection-string")]
    public async Task GetForeignKeysAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<object>> act = async () => await dbConnection.GetForeignKeysAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }
}
