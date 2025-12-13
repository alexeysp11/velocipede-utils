using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Metadata;

/// <summary>
/// Class for testing <see cref="PgDbConnection.GetColumns(string, out List{VelocipedeNativeColumnInfo})"/>.
/// </summary>
[Collection(nameof(PgDatabaseFixtureCollection))]
public sealed class PgGetColumnsTests : BaseGetColumnsTests
{
    /// <summary>
    /// Default constructor for creating <see cref="PgGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public PgGetColumnsTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
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
    value1 bytea
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
    value1 boolean,
    value2 bool
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
    value1 timestamp,
    value2 timestamp without time zone,
    value3 timestamp with time zone,
    value4 timestamptz,
    value5 date,
    value6 time,
    value7 time without time zone,
    value8 time with time zone,
    value9 interval,
    value10 interval (3),
    value11 interval hour to minute
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.DateTimeOffset, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.DateTimeOffset, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Date, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", ColumnType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value9", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value10", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value11", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
    value3 smallint,
    value4 bigint,
    value5 serial,
    value6 smallserial,
    value7 bigserial,
    value8 int2,
    value9 int8,
    value10 int4
)";

        // 2. Expected result.
        // In PostgreSQL, a primary key cannot be nullable because of its fundamental purpose in relational databases:
        // to uniquely identify each row in a table.
        // A SERIAL column in PostgreSQL is implicitly NOT NULL because its primary purpose is to provide a unique identifier
        // for each row.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = false },
            new() { ColumnName = "value6", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = false },
            new() { ColumnName = "value7", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = false },
            new() { ColumnName = "value8", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value9", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value10", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
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
    value7 money
)";

        // 2. Expected result.
        // If you specify the decimal type when creating a column, PostgreSQL for some reason returns a native datatype of numeric.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
    value2 double precision,
    value3 float4,
    value4 float8
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
    value1 character varying(55),
    value2 varchar(55),
    value3 character(55),
    value4 char(55),
    value5 bpchar(55),
    value6 bpchar,
    value7 text
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.String, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.String, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }
}
