using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.Models.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Metadata;

/// <summary>
/// Class for testing <see cref="SqliteDbConnection.GetColumns(string, out List{VelocipedeNativeColumnInfo})"/>.
/// </summary>
public sealed class SqliteGetColumnsTests : BaseGetColumnsTests, IClassFixture<SqliteDatabaseFixture>
{
    /// <summary>
    /// Default constructor for creating <see cref="SqliteGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public SqliteGetColumnsTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
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
    value1 BLOB
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
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
    value1 BOOLEAN,
    value2 BOOL,
    value3 BIT
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", ColumnType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
    value1 DATE,
    value2 DATETIME,
    value3 DATETIME2,
    value4 TIME,
    value5 TIMESTAMP
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", ColumnType = DbType.Date, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.DateTime2, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
    value2 INT,
    value3 INTEGER,
    value4 Integer,
    value5 tinyint,
    value6 TINYINT,
    value7 Tinyint,
    value8 smallint,
    value9 SMALLINT,
    value10 Smallint,
    value11 mediumint,
    value12 MEDIUMINT,
    value13 Mediumint,
    value14 bigint,
    value15 BIGINT,
    value16 Bigint,
    value17 unsigned big int,
    value18 UNSIGNED BIG INT,
    value19 UNSIGNED Big int,
    value20 int2,
    value21 INT2,
    value22 Int2,
    value23 int8,
    value24 INT8,
    value25 Int8,
    value26 int4,
    value27 INT4,
    value28 Int4,
    value29 unsigned bigint,
    value30 UNSIGNED BIGINT,
    value31 UNSIGNED Bigint,
    value32 unsigned integer,
    value33 UNSIGNED INTEGER,
    value34 UNSIGNED Integer
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value8", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value9", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value10", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value11", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value12", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value13", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value14", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value15", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value16", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value17", ColumnType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value18", ColumnType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value19", ColumnType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value20", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value21", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value22", ColumnType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value23", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value24", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value25", ColumnType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value26", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value27", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value28", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value29", ColumnType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value30", ColumnType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value31", ColumnType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value32", ColumnType = DbType.UInt32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value33", ColumnType = DbType.UInt32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value34", ColumnType = DbType.UInt32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
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
    value1 NUMERIC,
    value2 NUMERIC(10),
    value3 NUMERIC(10,5),
    value4 DECIMAL,
    value5 DECIMAL(10),
    value6 DECIMAL(10,5)
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.Decimal, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
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
    value1 REAL,
    value2 DOUBLE,
    value3 DOUBLE PRECISION,
    value4 FLOAT
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
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
    value1 CLOB,
    value2 TEXT,
    value3 text,
    value4 CHARACTER(55),
    value5 VARCHAR(55),
    value6 VARYING CHARACTER(55),
    value7 CHARACTER VARYING(55),
    value8 NATIVE CHARACTER(55),
    value9 NCHAR(55),
    value10 NVARCHAR(55),
    value11 CHAR(55),
    value12 CHARACTER,
    value13 VARCHAR,
    value14 VARYING CHARACTER,
    value15 CHARACTER VARYING,
    value16 NATIVE CHARACTER,
    value17 NCHAR,
    value18 NVARCHAR,
    value19 CHAR
)";

        // 2. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", ColumnType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value9", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value10", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value11", ColumnType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value12", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value13", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value14", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value15", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value16", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value17", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value18", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value19", ColumnType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }
}
