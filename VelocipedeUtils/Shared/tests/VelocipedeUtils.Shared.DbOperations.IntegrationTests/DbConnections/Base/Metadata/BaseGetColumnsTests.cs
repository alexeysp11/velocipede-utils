using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Enums;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
/// </summary>
public abstract class BaseGetColumnsTests : BaseDbConnectionTests
{
    /// <summary>
    /// Default constructor for creating <see cref="BaseGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
    protected BaseGetColumnsTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("TestModels")]
    [InlineData("testModels")]
    [InlineData("testmodels")]
    [InlineData("Testmodels")]
    [InlineData("TESTMODELS")]
    public void GetColumns_FixtureNotConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        
        // Act.
        dbConnection.GetColumns(tableName, out List<VelocipedeColumnInfo>? result);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("TestModels")]
    [InlineData("testModels")]
    [InlineData("testmodels")]
    [InlineData("Testmodels")]
    [InlineData("TESTMODELS")]
    public void GetColumns_FixtureConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        dbConnection
            .OpenDb()
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? result)
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(StringConversionType.None)]
    [InlineData(StringConversionType.ToLower)]
    [InlineData(StringConversionType.ToUpper)]
    public void GetColumns_FixtureConnectedCaseInsensitive(StringConversionType tableNameTransformationType)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = $"{nameof(GetColumns_FixtureConnectedCaseInsensitive)}_{tableNameTransformationType}";
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Table name transformation.
        string tableNameTransformed = tableNameTransformationType switch
        {
            StringConversionType.ToLower => tableName.ToLower(),
            StringConversionType.ToUpper => tableName.ToUpper(),
            _ => tableName,
        };

        // 4. Expected result.
        int expectedQty = 2;

        // Act.
        dbConnection
            .GetColumns(tableNameTransformed, out List<VelocipedeColumnInfo>? result)
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
    public void GetColumns_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException(string tableName)
    {
        // Arrange.
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

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("TestModels")]
    [InlineData("testModels")]
    [InlineData("testmodels")]
    [InlineData("Testmodels")]
    [InlineData("TESTMODELS")]
    public async Task GetColumnsAsync_FixtureNotConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // Act.
        List<VelocipedeColumnInfo>? result = await dbConnection.GetColumnsAsync(tableName);

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(3);
    }

    [Theory]
    [InlineData("\"TestModels\"")]
    [InlineData("TestModels")]
    [InlineData("testModels")]
    [InlineData("testmodels")]
    [InlineData("Testmodels")]
    [InlineData("TESTMODELS")]
    public async Task GetColumnsAsync_FixtureConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

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

    [Theory]
    [InlineData(StringConversionType.None)]
    [InlineData(StringConversionType.ToLower)]
    [InlineData(StringConversionType.ToUpper)]
    public async Task GetColumnsAsync_FixtureConnectedCaseInsensitive(StringConversionType tableNameTransformationType)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = $"{nameof(GetColumnsAsync_FixtureConnectedCaseInsensitive)}_{tableNameTransformationType}";
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Table name transformation.
        string tableNameTransformed = tableNameTransformationType switch
        {
            StringConversionType.ToLower => tableName.ToLower(),
            StringConversionType.ToUpper => tableName.ToUpper(),
            _ => tableName,
        };

        // 4. Expected result.
        int expectedQty = 2;

        // Act.
        List<VelocipedeColumnInfo>? result = await dbConnection.GetColumnsAsync(tableNameTransformed);
        dbConnection.CloseDb();

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
    public async Task GetColumnsAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException(string tableName)
    {
        // Arrange.
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
