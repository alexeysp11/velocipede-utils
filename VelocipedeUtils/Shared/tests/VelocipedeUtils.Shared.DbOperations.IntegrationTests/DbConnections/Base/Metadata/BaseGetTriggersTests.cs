using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.GetTriggers(string, out List{VelocipedeTriggerInfo})"/>.
/// </summary>
public abstract class BaseGetTriggersTests : BaseDbConnectionTests
{
    protected BaseGetTriggersTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public void GetTriggers_FixtureNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.GetTriggers(tableName, out List<VelocipedeTriggerInfo>? result);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public void GetTriggers_FixtureConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .GetTriggers(tableName, out List<VelocipedeTriggerInfo>? result)
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Fact]
    public void GetTriggers_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTriggers(tableName, out _);

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
    public void GetTriggers_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTriggers(tableName, out _);

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
    public void GetTriggers_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetTriggers(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public async Task GetTriggersAsync_FixtureNotConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeTriggerInfo>? result = await dbConnection.GetTriggersAsync(tableName);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("\"TestModels\"", 0)]
    [InlineData("\"TestUsers\"", 2)]
    public async Task GetTriggersAsync_FixtureConnected_ForeignKeyQtyEqualsToExpected(string tableName, int expectedQty)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeTriggerInfo>? result = await dbConnection
            .OpenDb()
            .GetTriggersAsync(tableName);
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Fact]
    public async Task GetTriggersAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeTriggerInfo>>> act = async () => await dbConnection.GetTriggersAsync(tableName);

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
    public async Task GetTriggersAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeTriggerInfo>>> act = async () => await dbConnection.GetTriggersAsync(tableName);

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
    public async Task GetTriggersAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeTriggerInfo>>> act = async () => await dbConnection.GetTriggersAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }
}
