using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Metadata;

/// <summary>
/// Class for testing <see cref="MssqlDbConnection.GetColumns(string, out List{VelocipedeNativeColumnInfo})"/>.
/// </summary>
[Collection(nameof(MssqlDatabaseFixtureCollection))]
public sealed class MssqlGetColumnsTests : BaseGetColumnsTests
{
    /// <summary>
    /// Default constructor for creating <see cref="MssqlGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public MssqlGetColumnsTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }

    [Fact]
    public override void GetColumns_Blob()
    {
        // Arrange.
        // 1. Table.
        string tableName = nameof(GetColumns_Blob);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 binary,
    value2 binary(10),
    value3 varbinary,
    value4 varbinary(10),
    value5 varbinary(max),
    value6 binary varying,
    value7 binary varying(10),
    value8 binary varying(max)
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }

    [Fact]
    public override void GetColumns_Boolean()
    {
        // Arrange.
        // 1. Table.
        string tableName = nameof(GetColumns_Boolean);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 bit
)";
        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", DbType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }

    [Fact]
    public override void GetColumns_Datetime()
    {
        // Arrange.
        // 1. Table.
        string tableName = nameof(GetColumns_Datetime);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 datetime,
    value2 datetime2,
    value3 datetimeoffset,
    value4 smalldatetime,
    value5 date,
    value6 time
)";
        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", DbType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.DateTime2, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.DateTimeOffset, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.Date, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }

    [Fact]
    public override void GetColumns_Integer()
    {
        // Arrange.
        // 1. Table.
        string tableName = nameof(GetColumns_Integer);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 integer,
    value2 int,
    value3 tinyint,
    value4 smallint,
    value5 bigint
)";

        // 2. Expected result.
        // In MS SQL, a primary key cannot be nullable because of its fundamental purpose in relational databases:
        // to uniquely identify each row in a table.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }

    [Fact]
    public override void GetColumns_Numeric()
    {
        // Arrange.
        // 1. Table.
        string tableName = nameof(GetColumns_Numeric);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 numeric,
    value2 numeric(10),
    value3 numeric(10,5),
    value4 decimal,
    value5 decimal(10),
    value6 decimal(10,5),
    value7 dec
)";

        // 2. Expected result.
        // If you specify the decimal type when creating a column, MS SQL for some reason returns a native datatype of decimal.
        // If you declare a DECIMAL without specifying precision and scale (e.g., just DECIMAL), SQL Server defaults to DECIMAL(18,0).
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", DbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value7", DbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }

    [Fact]
    public override void GetColumns_Real()
    {
        // Arrange.
        // 1. Table.
        string tableName = nameof(GetColumns_Real);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 real,
    value2 float,
    value3 float(12),
    value4 float(24),
    value5 float(30),
    value6 float(53),
    value7 double precision
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }

    [Fact]
    public override void GetColumns_Text()
    {
        // Arrange.
        // 1. Table.
        string tableName = nameof(GetColumns_Text);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 text,
    value2 char(55),
    value3 varchar(55),
    value4 varchar(max),
    value5 char varying,
    value6 character,
    value7 character(55),
    value8 character varying(55),
    value9 ntext,
    value10 nchar(55),
    value11 nvarchar(55),
    value12 nvarchar(max),
    value13 national character(55),
    value14 national char(55),
    value15 national character varying(55),
    value16 national char varying(55),
    value17 national text
)";

        // 2. Expected result.
        int textColumnSize = int.MaxValue;
        int nationalTextColumnSize = 1073741823;
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", DbType = DbType.String, CharMaxLength = textColumnSize, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.String, CharMaxLength = 1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.String, CharMaxLength = 1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value9", DbType = DbType.String, CharMaxLength = nationalTextColumnSize, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value10", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value11", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value12", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value13", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value14", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value15", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value16", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value17", DbType = DbType.String, CharMaxLength = nationalTextColumnSize, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }
}
