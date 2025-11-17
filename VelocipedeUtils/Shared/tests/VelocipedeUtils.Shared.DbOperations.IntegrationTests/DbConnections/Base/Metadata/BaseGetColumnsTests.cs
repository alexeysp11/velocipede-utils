using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
/// </summary>
public abstract class BaseGetColumnsTests : BaseDbConnectionTests
{
    protected BaseGetColumnsTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Fact]
    public void GetColumns_FixtureNotConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        dbConnection.GetColumns(tableName, out List<VelocipedeColumnInfo>? result);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public void GetColumns_FixtureConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        dbConnection
            .OpenDb()
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? result)
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public void GetColumns_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetColumns(tableName, out _);

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
    public void GetColumns_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetColumns(tableName, out _);

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
    public void GetColumns_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetColumns(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Fact]
    public async Task GetColumnsAsync_FixtureNotConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        List<VelocipedeColumnInfo>? result = await dbConnection.GetColumnsAsync(tableName);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetColumnsAsync_FixtureConnected_ResultContainsAllExpectedStrings()
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        string tableName = "\"TestModels\"";

        // Act.
        List<VelocipedeColumnInfo>? result = await dbConnection
            .OpenDb()
            .GetColumnsAsync(tableName);
        dbConnection
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetColumnsAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeColumnInfo>>> act = async () => await dbConnection.GetColumnsAsync(tableName);

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
    public async Task GetColumnsAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<List<VelocipedeColumnInfo>>> act = async () => await dbConnection.GetColumnsAsync(tableName);

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
    public async Task GetColumnsAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<object>> act = async () => await dbConnection.GetColumnsAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }
}
