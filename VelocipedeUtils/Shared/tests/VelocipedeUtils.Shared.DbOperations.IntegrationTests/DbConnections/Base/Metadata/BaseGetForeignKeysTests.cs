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
/// Base class for testing <see cref="IVelocipedeDbConnection.GetForeignKeys(string, out List{VelocipedeForeignKeyInfo})"/>.
/// </summary>
public abstract class BaseGetForeignKeysTests : BaseDbConnectionTests
{
    /// <summary>
    /// Default constructor for creating <see cref="BaseGetForeignKeysTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
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
    [InlineData(null)]
    [InlineData("")]
    public void GetForeignKeys_NullOrEmptyTable_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<IVelocipedeDbConnection> act = () => dbConnection.GetForeignKeys(tableName, out _);

        // Act & Assert.
        act
            .Should()
            .Throw<ArgumentNullException>()
            .WithMessage(ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
    }

    [Theory]
    [InlineData(CaseConversionType.None)]
    [InlineData(CaseConversionType.ToLower)]
    [InlineData(CaseConversionType.ToUpper)]
    [InlineData(CaseConversionType.None, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(CaseConversionType.ToLower, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(CaseConversionType.ToUpper, DelimitIdentifierType.DoubleQuotes)]
    public void GetForeignKeys_CaseInsensitive(
        CaseConversionType conversionType,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetForeignKeys_CaseInsensitive),
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
            .GetForeignKeys(tableNameConverted, out List<VelocipedeForeignKeyInfo>? result)
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
    [InlineData(null)]
    [InlineData("")]
    public async Task GetForeignKeysAsync_NullOrEmptyTable_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<Task<List<VelocipedeForeignKeyInfo>>> act = async () => await dbConnection.GetForeignKeysAsync(tableName);

        // Act & Assert.
        await act
            .Should()
            .ThrowAsync<ArgumentNullException>()
            .WithMessage(ErrorMessageConstants.TableNameCouldNotBeNullOrEmpty);
    }

    [Theory]
    [InlineData(CaseConversionType.None)]
    [InlineData(CaseConversionType.ToLower)]
    [InlineData(CaseConversionType.ToUpper)]
    [InlineData(CaseConversionType.None, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(CaseConversionType.ToLower, DelimitIdentifierType.DoubleQuotes)]
    [InlineData(CaseConversionType.ToUpper, DelimitIdentifierType.DoubleQuotes)]
    public async Task GetForeignKeysAsync_CaseInsensitive(
        CaseConversionType conversionType,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetForeignKeysAsync_CaseInsensitive),
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
        List<VelocipedeForeignKeyInfo>? result = await dbConnection.GetForeignKeysAsync(tableNameConverted);
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
