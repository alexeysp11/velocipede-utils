using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Exceptions;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Helpers;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

/// <summary>
/// Base class for testing <see cref="IVelocipedeDbConnection.GetTriggers(string, out List{VelocipedeTriggerInfo})"/>.
/// </summary>
public abstract class BaseGetTriggersTests : BaseDbConnectionTests
{
    /// <summary>
    /// Default constructor for creating <see cref="BaseGetTriggersTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
    protected BaseGetTriggersTests(IDatabaseFixture fixture, string createDatabaseSql)
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
    [InlineData("\"TestUsers\"", 2)]
    [InlineData("TestUsers", 2)]
    [InlineData("testUsers", 2)]
    [InlineData("testusers", 2)]
    [InlineData("Testusers", 2)]
    [InlineData("TESTUSERS", 2)]
    public void GetTriggers_FixtureNotConnected(string tableName, int expectedQty)
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
    [InlineData("TestModels", 0)]
    [InlineData("testModels", 0)]
    [InlineData("testmodels", 0)]
    [InlineData("Testmodels", 0)]
    [InlineData("TESTMODELS", 0)]
    [InlineData("\"TestUsers\"", 2)]
    [InlineData("TestUsers", 2)]
    [InlineData("testUsers", 2)]
    [InlineData("testusers", 2)]
    [InlineData("Testusers", 2)]
    [InlineData("TESTUSERS", 2)]
    public void GetTriggers_FixtureConnected(string tableName, int expectedQty)
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

    [Theory]
    [InlineData(StringConversionType.None)]
    [InlineData(StringConversionType.ToLower)]
    [InlineData(StringConversionType.ToUpper)]
    [InlineData(StringConversionType.None, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(StringConversionType.ToLower, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(StringConversionType.ToUpper, DelimitIdentifierType.DoubleQuotes)]
    public void GetTriggers_CaseInsensitive(
        StringConversionType conversionType,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetTriggers_CaseInsensitive),
            conversionType: conversionType,
            delimitIdentifierType: delimitIdentifierType);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Table name transformation.
        string tableNameConverted = TableNameHelper.ConvertTableName(
            tableName,
            dbConnection.DatabaseType,
            conversionType,
            delimitIdentifierType);

        // 4. Expected result.
        int expectedQty = 0;

        // Act.
        dbConnection
            .GetTriggers(tableName, out List<VelocipedeTriggerInfo>? result)
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
    [InlineData("---")]
    public void GetTriggers_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException(string tableName)
    {
        // Arrange.
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
    [InlineData("TestModels", 0)]
    [InlineData("testModels", 0)]
    [InlineData("testmodels", 0)]
    [InlineData("Testmodels", 0)]
    [InlineData("TESTMODELS", 0)]
    [InlineData("\"TestUsers\"", 2)]
    [InlineData("TestUsers", 2)]
    [InlineData("testUsers", 2)]
    [InlineData("testusers", 2)]
    [InlineData("Testusers", 2)]
    [InlineData("TESTUSERS", 2)]
    public async Task GetTriggersAsync_FixtureNotConnected(string tableName, int expectedQty)
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
    [InlineData("TestModels", 0)]
    [InlineData("testModels", 0)]
    [InlineData("testmodels", 0)]
    [InlineData("Testmodels", 0)]
    [InlineData("TESTMODELS", 0)]
    [InlineData("\"TestUsers\"", 2)]
    [InlineData("TestUsers", 2)]
    [InlineData("testUsers", 2)]
    [InlineData("testusers", 2)]
    [InlineData("Testusers", 2)]
    [InlineData("TESTUSERS", 2)]
    public async Task GetTriggersAsync_FixtureConnected(string tableName, int expectedQty)
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

    [Theory]
    [InlineData(StringConversionType.None)]
    [InlineData(StringConversionType.ToLower)]
    [InlineData(StringConversionType.ToUpper)]
    [InlineData(StringConversionType.None, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(StringConversionType.ToLower, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(StringConversionType.ToUpper, DelimitIdentifierType.DoubleQuotes)]
    public async Task GetTriggersAsync_CaseInsensitive(
        StringConversionType conversionType,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetTriggersAsync_CaseInsensitive),
            conversionType: conversionType,
            delimitIdentifierType: delimitIdentifierType);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Table name transformation.
        string tableNameConverted = TableNameHelper.ConvertTableName(
            tableName,
            dbConnection.DatabaseType,
            conversionType,
            delimitIdentifierType);

        // 4. Expected result.
        int expectedQty = 0;

        // Act.
        List<VelocipedeTriggerInfo>? result = await dbConnection.GetTriggersAsync(tableName);
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
    [InlineData("---")]
    public async Task GetTriggersAsync_GuidInsteadOfConnectionString_ThrowsVelocipedeDbConnectParamsException(string tableName)
    {
        // Arrange.
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
