using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.GetSqlDefinition(string, out string?)"/>.
/// </summary>
public abstract class BaseGetSqlDefinitionTests : BaseDbConnectionTests
{
    private readonly string _createTestModelsSql;
    private readonly string _createTestUsersSql;

    /// <summary>
    /// Default constructor for creating <see cref="BaseGetSqlDefinitionTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
    protected BaseGetSqlDefinitionTests(
        IDatabaseFixture fixture,
        string createDatabaseSql,
        string createTestModelsSql,
        string createTestUsersSql) : base(fixture, createDatabaseSql)
    {
        _createTestModelsSql = createTestModelsSql;
        _createTestUsersSql = createTestUsersSql;
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual void GetSqlDefinition_FixtureNotConnected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection.GetSqlDefinition(tableName, out string? result);
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual void GetSqlDefinition_FixtureConnected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .GetSqlDefinition(tableName, out string? result)
            .CloseDb();
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetSqlDefinition_NullOrEmptyTable_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<IVelocipedeDbConnection> act = () => dbConnection.GetSqlDefinition(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage(ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
    }

    [Fact]
    public void GetSqlDefinition_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

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
    public void GetSqlDefinition_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

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
    public void GetSqlDefinition_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Action act = () => dbConnection.GetSqlDefinition(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual async Task GetSqlDefinitionAsync_FixtureNotConnected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        string? result = await dbConnection.GetSqlDefinitionAsync(tableName);
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
    public virtual async Task GetSqlDefinitionAsync_FixtureConnected(string tableName)
    {
        // Arrange.
        string expected = GetExpectedSqlDefinition(tableName);
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        string? result = await dbConnection
            .OpenDb()
            .GetSqlDefinitionAsync(tableName);
        dbConnection
            .CloseDb();
        result = result?.Replace("\r\n", "\n");
        expected = expected.Replace("\r\n", "\n");

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetSqlDefinitionAsync_NullOrEmptyTable_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<Task<string?>> act = async () => await dbConnection.GetSqlDefinitionAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<ArgumentNullException>()
            .WithMessage(ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
    }

    [Fact]
    public async Task GetSqlDefinitionAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException()
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        string connectionString = Guid.NewGuid().ToString();
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<string?>> act = async () => await dbConnection.GetSqlDefinitionAsync(tableName);

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
    public async Task GetSqlDefinitionAsync_NullOrEmptyConnectionString_ThrowsInvalidOperationException(string? connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<string?>> act = async () => await dbConnection.GetSqlDefinitionAsync(tableName);

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
    public async Task GetSqlDefinitionAsync_IncorrectConnectionString_ThrowsVelocipedeDbConnectParamsException(string connectionString)
    {
        // Arrange.
        string tableName = "\"TestModels\"";
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        dbConnection.SetConnectionString(connectionString);
        Func<Task<string?>> act = async () => await dbConnection.GetSqlDefinitionAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<VelocipedeDbConnectParamsException>()
            .WithInnerException(typeof(ArgumentException));
        dbConnection.IsConnected.Should().BeFalse();
    }

    private string GetExpectedSqlDefinition(string tableName)
    {
        tableName = tableName.Trim('"');
        return tableName switch
        {
            "TestModels" => _createTestModelsSql,
            "TestUsers" => _createTestUsersSql,
            _ => "",
        };
    }
}
