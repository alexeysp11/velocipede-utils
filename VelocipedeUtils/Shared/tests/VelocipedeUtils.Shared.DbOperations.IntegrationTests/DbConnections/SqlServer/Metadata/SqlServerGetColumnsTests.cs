using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Metadata;

/// <summary>
/// Class for testing <see cref="SqlServerDbConnection.GetColumns(string, out List{VelocipedeNativeColumnInfo})"/>.
/// </summary>
[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerGetColumnsTests : BaseGetColumnsTests
{
    /// <summary>
    /// Default constructor for creating <see cref="SqlServerGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public SqlServerGetColumnsTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
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
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.DateTime2, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.DateTimeOffset, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Date, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
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
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
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
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.String, CharMaxLength = textColumnSize, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.String, CharMaxLength = 1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.String, CharMaxLength = 1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value9", ColumnType = DbType.String, CharMaxLength = nationalTextColumnSize, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value10", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value11", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value12", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value13", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value14", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value15", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value16", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value17", ColumnType = DbType.String, CharMaxLength = nationalTextColumnSize, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }
}
