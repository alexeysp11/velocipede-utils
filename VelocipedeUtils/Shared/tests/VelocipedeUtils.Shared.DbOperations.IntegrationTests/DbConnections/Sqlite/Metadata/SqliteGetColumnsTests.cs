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
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", DbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", DbType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", DbType = DbType.Date, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.DateTime2, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value7", DbType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value8", DbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value9", DbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value10", DbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value11", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value12", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value13", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value14", DbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value15", DbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value16", DbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value17", DbType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value18", DbType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value19", DbType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value20", DbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value21", DbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value22", DbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value23", DbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value24", DbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value25", DbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value26", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value27", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value28", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value29", DbType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value30", DbType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value31", DbType = DbType.UInt64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value32", DbType = DbType.UInt32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value33", DbType = DbType.UInt32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value34", DbType = DbType.UInt32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
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
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", DbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
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
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
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
            new() { ColumnName = "id", DbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value9", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value10", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value11", DbType = DbType.String, CharMaxLength = 55, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value12", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value13", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value14", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value15", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value16", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value17", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value18", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value19", DbType = DbType.String, CharMaxLength = -1, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act & Assert.
        ValidateGetColumnsTest(sql, tableName, expected);
    }
}
