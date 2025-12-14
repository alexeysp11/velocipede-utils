using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;
using VelocipedeUtils.Shared.DbOperations.QueryBuilders;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.QueryBuilders;

/// <summary>
/// Integration tests for <see cref="ICreateTableQueryBuilder"/>.
/// </summary>
/// <remarks>
/// The integration tests for <see cref="ICreateTableQueryBuilder"/> must validate the correctness of the table creation
/// in the database.
/// An approximate validation algorithm should include:
/// <list type="number">
/// <item><description>Constructing and executing a query;</description></item>
/// <item><description>Obtaining metadata about the table;</description></item>
/// <item><description>Comparing the metadata with the expected collection.</description></item>
/// </list>
/// </remarks>
[Collection(nameof(DatabaseFixtureResolverCollection))]
public sealed class CreateTableQueryBuilderTests
{
    private const string COLUMN_NAME = "ColumnName1";

    private readonly DatabaseFixtureResolver _fixtureResolver;

    public CreateTableQueryBuilderTests(DatabaseFixtureResolver fixtureResolver)
    {
        _fixtureResolver = fixtureResolver;
    }

    #region Test cases
    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoInteger(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.SByte },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Byte },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Int16 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.UInt16 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Int32 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.UInt32 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Int64 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.UInt64 },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoText(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.String, CharMaxLength = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, CharMaxLength = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiString, CharMaxLength = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.String, CharMaxLength = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, CharMaxLength = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiString, CharMaxLength = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.String, CharMaxLength = -50 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, CharMaxLength = -50 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiString, CharMaxLength = -50 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -50 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.String, CharMaxLength = 50 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.StringFixedLength, CharMaxLength = 50 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiString, CharMaxLength = 50 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 50 },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoBlob(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Binary },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoReal(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Double },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Single },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoNumeric(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 5 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 5 },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoCurrency(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = null, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = -1, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 0, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = null },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = -1 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 0 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 2 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 4 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Currency, NumericPrecision = 10, NumericScale = 5 },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoBoolean(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Boolean },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoDatetime(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Time },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Date },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.DateTime },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.DateTime2 },
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.DateTimeOffset },
    ];

    public static TheoryData<VelocipedeColumnInfo> GetColumnInfoGuid(VelocipedeDatabaseType databaseType) => [
        new() { ColumnName = COLUMN_NAME, DatabaseType = databaseType, ColumnType = DbType.Guid },
    ];
    #endregion  // Test cases

    private static List<DbType> GetExpectedDbTypes(DbType? input)
    {
        ArgumentNullException.ThrowIfNull(input);

        return input switch
        {
            DbType.SByte or DbType.Byte => [DbType.SByte, DbType.Byte, DbType.Int16],
            DbType.UInt16 => [DbType.UInt16, DbType.Int16],
            DbType.Int16 => [DbType.Int16],
            DbType.UInt32 => [DbType.UInt32, DbType.Int32],
            DbType.Int32 => [DbType.Int32],
            DbType.UInt64 => [DbType.UInt64, DbType.Int64],
            DbType.Int64 => [DbType.Int64],
            DbType.VarNumeric or DbType.Decimal or DbType.Currency => [DbType.VarNumeric, DbType.Decimal, DbType.Currency],
            DbType.DateTime or DbType.DateTime2 or DbType.DateTimeOffset => [DbType.DateTime, DbType.DateTime2, DbType.DateTimeOffset],
            DbType.AnsiString or DbType.AnsiStringFixedLength or DbType.String or DbType.StringFixedLength => [DbType.String],
            DbType.Single => [DbType.Double],
            DbType.Guid => [DbType.Guid, DbType.String],
            _ => [(DbType)input]
        };
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoInteger), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoInteger), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoInteger), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Integer(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    [Theory]
    [MemberData(nameof(GetColumnInfoText), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoText), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoText), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Text(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    [Theory]
    [MemberData(nameof(GetColumnInfoBlob), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoBlob), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoBlob), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Blob(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    [Theory]
    [MemberData(nameof(GetColumnInfoReal), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoReal), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoReal), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Real(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    [Theory]
    [MemberData(nameof(GetColumnInfoNumeric), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoNumeric), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoNumeric), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Numeric(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);
    
    [Theory]
    [MemberData(nameof(GetColumnInfoCurrency), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoCurrency), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoCurrency), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Currency(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    [Theory]
    [MemberData(nameof(GetColumnInfoBoolean), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoBoolean), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoBoolean), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Boolean(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    [Theory]
    [MemberData(nameof(GetColumnInfoDatetime), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoDatetime), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoDatetime), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Datetime(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    [Theory]
    [MemberData(nameof(GetColumnInfoGuid), parameters: VelocipedeDatabaseType.SQLite)]
    [MemberData(nameof(GetColumnInfoGuid), parameters: VelocipedeDatabaseType.PostgreSQL)]
    [MemberData(nameof(GetColumnInfoGuid), parameters: VelocipedeDatabaseType.MSSQL)]
    public async Task BuildAndToString_Guid(VelocipedeColumnInfo columnInfo)
        => await ValidateBuildAndToString(columnInfo);

    private async Task ValidateBuildAndToString(VelocipedeColumnInfo columnInfo)
    {
        // Arrange.
        string tableName = Guid.NewGuid().ToString();
        List<DbType> expectedColumnTypes = GetExpectedDbTypes(columnInfo.ColumnType);
        VelocipedeDatabaseType databaseType = columnInfo.DatabaseType;
        IDatabaseFixture fixture = _fixtureResolver.GetFixture(databaseType);

        // Act.
        // 1. Get create table statement using query builder.
        CreateTableQueryBuilder queryBuilder = new(databaseType, tableName);
        string? sql = queryBuilder
            .WithColumn(columnInfo)
            .Build()
            .ToString();

        // 2. Use database connection to create table and get the metadata.
        IVelocipedeDbConnection dbConnection = fixture.GetVelocipedeDbConnection();
#nullable disable
        await dbConnection.ExecuteAsync(sql);
#nullable restore
        List<VelocipedeNativeColumnInfo> resultColumns = await dbConnection.GetColumnsAsync(tableName);
        VelocipedeNativeColumnInfo? createdColumn = resultColumns.FirstOrDefault();

        // Assert.
        sql
            .Should()
            .NotBeNullOrEmpty();
        resultColumns
            .Should()
            .NotBeNull()
            .And
            .HaveCount(1);
        createdColumn
            .Should()
            .NotBeNull();
        createdColumn.ColumnName
            .Should()
            .Be(columnInfo.ColumnName);
        createdColumn.DatabaseType
            .Should()
            .Be(databaseType);
        createdColumn.CalculatedColumnType
            .Should()
            .NotBeNull()
            .And
            .BeOneOf(expectedColumnTypes);
    }
}
