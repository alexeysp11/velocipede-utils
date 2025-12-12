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
    public static TheoryData<VelocipedeColumnInfo> ColumnInfoInteger => [
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.SByte },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Byte },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Int16 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.UInt16 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Int32 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.UInt32 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Int64 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.UInt64 },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.SByte },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Byte },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Int16 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.UInt16 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Int32 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.UInt32 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Int64 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.UInt64 },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.SByte },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Byte },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Int16 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.UInt16 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Int32 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.UInt32 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Int64 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.UInt64 },
        ];

    public static TheoryData<VelocipedeColumnInfo> ColumnInfoText => [
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.String, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.StringFixedLength, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiString, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 50 },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.String, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiString, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 50 },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = -50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.String, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.StringFixedLength, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiString, CharMaxLength = 50 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.AnsiStringFixedLength, CharMaxLength = 50 },
        ];

    public static TheoryData<VelocipedeColumnInfo> ColumnInfoBlob => [
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Binary },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Binary },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Binary },
        ];

    public static TheoryData<VelocipedeColumnInfo> ColumnInfoReal => [
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Double },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Single },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Double },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Single },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Double },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Single },
        ];

    public static TheoryData<VelocipedeColumnInfo> ColumnInfoNumeric => [
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 5 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 5 },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 5 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 5 },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = -1, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = -1, NumericScale = -1 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = null, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = null, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 0, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 0, NumericScale = 0 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = null },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.VarNumeric, NumericPrecision = 10, NumericScale = 5 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Decimal, NumericPrecision = 10, NumericScale = 5 },
        ];

    public static TheoryData<VelocipedeColumnInfo> ColumnInfoBoolean => [
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Boolean },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Boolean },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Boolean },
        ];

    public static TheoryData<VelocipedeColumnInfo> ColumnInfoDatetime => [
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Time },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Date },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.DateTime },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.DateTime2 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.DateTimeOffset },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Time },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Date },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.DateTime },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.DateTime2 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.DateTimeOffset },

            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Time },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Date },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.DateTime },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.DateTime2 },
            new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.DateTimeOffset },
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
            DbType.VarNumeric or DbType.Decimal => [DbType.VarNumeric, DbType.Decimal],
            DbType.DateTime or DbType.DateTime2 or DbType.DateTimeOffset => [DbType.DateTime, DbType.DateTime2, DbType.DateTimeOffset],
            DbType.AnsiString or DbType.AnsiStringFixedLength or DbType.String or DbType.StringFixedLength => [DbType.String],
            DbType.Single => [DbType.Double],
            _ => [(DbType)input]
        };
    }

    [Theory]
    [MemberData(nameof(ColumnInfoInteger))]
    [MemberData(nameof(ColumnInfoText))]
    [MemberData(nameof(ColumnInfoBlob))]
    [MemberData(nameof(ColumnInfoReal))]
    [MemberData(nameof(ColumnInfoNumeric))]
    [MemberData(nameof(ColumnInfoBoolean))]
    [MemberData(nameof(ColumnInfoDatetime))]
    public async Task BuildAndToString(VelocipedeColumnInfo columnInfo)
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
