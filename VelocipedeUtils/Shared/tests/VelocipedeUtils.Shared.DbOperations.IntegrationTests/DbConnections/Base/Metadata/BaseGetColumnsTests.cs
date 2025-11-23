using System.Data;
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
/// Base class for testing <see cref="IVelocipedeDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
/// </summary>
public abstract class BaseGetColumnsTests : BaseDbConnectionTests
{
    /// <summary>
    /// An alternative to the <see cref="VelocipedeColumnInfo"/> class used for validating metadata in tests.
    /// </summary>
    private sealed class TestColumnInfo
    {
        /// <summary>
        /// Column name.
        /// </summary>
        public string? ColumnName { get; set; }

        /// <summary>
        /// The calculated type of the column.
        /// </summary>
        public DbType? CalculatedDbType { get; set; }

        /// <summary>
        /// If native column type identifies a character or bit string type, the declared maximum length;
        /// <c>null</c> for all other data types or if no maximum length was declared.
        /// </summary>
        public int? CharMaxLength { get; set; }

        /// <summary>
        /// Numeric precision for Decimal/Numeric.
        /// </summary>
        public int? NumericPrecision { get; set; }

        /// <summary>
        /// Numeric scale for Decimal/Numeric.
        /// </summary>
        public int? NumericScale { get; set; }

        /// <summary>
        /// Default value of the column.
        /// </summary>
        public object? DefaultValue { get; set; }

        /// <summary>
        /// Whether the column is a primary key.
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// Whether the column is nullable.
        /// </summary>
        public bool IsNullable { get; set; }
    }

    private readonly List<TestColumnInfo> _expectedTestModelColumnInfos;

    /// <summary>
    /// Default constructor for creating <see cref="BaseGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    /// <param name="createDatabaseSql">SQL query to create database.</param>
    protected BaseGetColumnsTests(IDatabaseFixture fixture, string createDatabaseSql)
        : base(fixture, createDatabaseSql)
    {
        _expectedTestModelColumnInfos = [
            new()
            {
                ColumnName = "Id",
                CalculatedDbType = DbType.Int32,
                IsPrimaryKey = true,
                IsNullable = false
            },
            new()
            {
                ColumnName = "Name",
                CalculatedDbType = DbType.String,
                CharMaxLength = 50,
                IsPrimaryKey = false,
                IsNullable = false
            },
            new()
            {
                ColumnName = "AdditionalInfo",
                CalculatedDbType = DbType.String,
                CharMaxLength = 50,
                IsPrimaryKey = false,
                IsNullable = true
            },
        ];
    }

    [Theory]
    [InlineData("TestModels")]
    [InlineData("\"TestModels\"")]
    [InlineData("testModels")]
    [InlineData("\"testModels\"")]
    [InlineData("testmodels")]
    [InlineData("\"testmodels\"")]
    [InlineData("Testmodels")]
    [InlineData("\"Testmodels\"")]
    [InlineData("TESTMODELS")]
    [InlineData("\"TESTMODELS\"")]
    public void GetColumns_FixtureNotConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestColumnInfo> expected = _expectedTestModelColumnInfos;

        // Act.
        dbConnection.GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo);
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("TestModels")]
    [InlineData("\"TestModels\"")]
    [InlineData("testModels")]
    [InlineData("\"testModels\"")]
    [InlineData("testmodels")]
    [InlineData("\"testmodels\"")]
    [InlineData("Testmodels")]
    [InlineData("\"Testmodels\"")]
    [InlineData("TESTMODELS")]
    [InlineData("\"TESTMODELS\"")]
    public void GetColumns_FixtureConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestColumnInfo> expected = _expectedTestModelColumnInfos;

        // Act.
        dbConnection
            .OpenDb()
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo)
            .CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GetColumns_NullOrEmptyTable_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<IVelocipedeDbConnection> act = () => dbConnection.GetColumns(tableName, out _);

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
    public void GetColumns_CaseInsensitive(
        CaseConversionType conversionType,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetColumns_CaseInsensitive),
            conversionType: conversionType,
            delimitIdentifierType: delimitIdentifierType);
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute($"create table {tableName} (id int, value varchar(50))")
            .Execute($"insert into {tableName} values (1, 'value 1'), (2, 'value 2'), (3, 'value 3'), (4, 'value 4')")
            .CommitTransaction();

        // 3. Table name conversion.
        string tableNameConverted = TableNameHelper.ConvertTableName(
            tableName,
            dbConnection.DatabaseType,
            conversionType,
            delimitIdentifierType);

        // 4. Expected result.
        int expectedQty = 2;

        // Act.
        dbConnection
            .GetColumns(tableNameConverted, out List<VelocipedeColumnInfo>? result)
            .CloseDb();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().HaveCount(expectedQty);
    }

    [Theory]
    [InlineData("TestModels")]
    [InlineData("\"TestModels\"")]
    [InlineData("testModels")]
    [InlineData("\"testModels\"")]
    [InlineData("testmodels")]
    [InlineData("\"testmodels\"")]
    [InlineData("Testmodels")]
    [InlineData("\"Testmodels\"")]
    [InlineData("TESTMODELS")]
    [InlineData("\"TESTMODELS\"")]
    [InlineData("---")]
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
    [InlineData("TestModels")]
    [InlineData("\"TestModels\"")]
    [InlineData("testModels")]
    [InlineData("\"testModels\"")]
    [InlineData("testmodels")]
    [InlineData("\"testmodels\"")]
    [InlineData("Testmodels")]
    [InlineData("\"Testmodels\"")]
    [InlineData("TESTMODELS")]
    [InlineData("\"TESTMODELS\"")]
    public async Task GetColumnsAsync_FixtureNotConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestColumnInfo> expected = _expectedTestModelColumnInfos;

        // Act.
        List<VelocipedeColumnInfo>? columnInfo = await dbConnection.GetColumnsAsync(tableName);
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData("TestModels")]
    [InlineData("\"TestModels\"")]
    [InlineData("testModels")]
    [InlineData("\"testModels\"")]
    [InlineData("testmodels")]
    [InlineData("\"testmodels\"")]
    [InlineData("Testmodels")]
    [InlineData("\"Testmodels\"")]
    [InlineData("TESTMODELS")]
    [InlineData("\"TESTMODELS\"")]
    public async Task GetColumnsAsync_FixtureConnected(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        List<TestColumnInfo> expected = _expectedTestModelColumnInfos;

        // Act.
        dbConnection.OpenDb();
        List<VelocipedeColumnInfo>? columnInfo = await dbConnection.GetColumnsAsync(tableName);
        dbConnection.CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetColumnsAsync_NullOrEmptyTable_ThrowsArgumentNullException(string tableName)
    {
        // Arrange.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();
        Func<Task<List<VelocipedeColumnInfo>>> act = async () => await dbConnection.GetColumnsAsync(tableName);

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
    public async Task GetColumnsAsync_CaseInsensitive(
        CaseConversionType conversionType,
        DelimitIdentifierType delimitIdentifierType = DelimitIdentifierType.None)
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = TableNameHelper.GetTableNameByTestMethod(
            methodName: nameof(GetColumnsAsync_CaseInsensitive),
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
        int expectedQty = 2;

        // Act.
        List<VelocipedeColumnInfo>? result = await dbConnection.GetColumnsAsync(tableNameConverted);
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
