using FluentAssertions;
using System.Data;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Metadata;

/// <summary>
/// Class for testing <see cref="MssqlDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
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
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
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
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value8", CalculatedDbType = DbType.Binary, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act.
        dbConnection
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo)
            .CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                NumericPrecision = x.NumericPrecision,
                NumericScale = x.NumericScale,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public override void GetColumns_Boolean()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = nameof(GetColumns_Boolean);
        string sql = $@"
create table {tableName} (
    id integer primary key,
    value1 bit
)";
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", CalculatedDbType = DbType.Boolean, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act.
        dbConnection
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo)
            .CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                NumericPrecision = x.NumericPrecision,
                NumericScale = x.NumericScale,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public override void GetColumns_Datetime()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
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
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", CalculatedDbType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", CalculatedDbType = DbType.DateTime2, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", CalculatedDbType = DbType.DateTimeOffset, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", CalculatedDbType = DbType.DateTime, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", CalculatedDbType = DbType.Date, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", CalculatedDbType = DbType.Time, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act.
        dbConnection
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo)
            .CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                NumericPrecision = x.NumericPrecision,
                NumericScale = x.NumericScale,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public override void GetColumns_Integer()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
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
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        // In MS SQL, a primary key cannot be nullable because of its fundamental purpose in relational databases:
        // to uniquely identify each row in a table.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", CalculatedDbType = DbType.SByte, CharMaxLength = null, NumericPrecision = 8, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value4", CalculatedDbType = DbType.Int16, CharMaxLength = null, NumericPrecision = 16, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", CalculatedDbType = DbType.Int64, CharMaxLength = null, NumericPrecision = 64, NumericScale = 0, IsNullable = true },
        ];

        // Act.
        dbConnection
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo)
            .CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                NumericPrecision = x.NumericPrecision,
                NumericScale = x.NumericScale,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public override void GetColumns_Numeric()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
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
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        // If you specify the decimal type when creating a column, MS SQL for some reason returns a native datatype of decimal.
        // If you declare a DECIMAL without specifying precision and scale (e.g., just DECIMAL), SQL Server defaults to DECIMAL(18,0).
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", CalculatedDbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value2", CalculatedDbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value3", CalculatedDbType = DbType.VarNumeric, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value4", CalculatedDbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value5", CalculatedDbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 0, IsNullable = true },
            new() { ColumnName = "value6", CalculatedDbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 10, NumericScale = 5, IsNullable = true },
            new() { ColumnName = "value7", CalculatedDbType = DbType.Decimal, CharMaxLength = null, NumericPrecision = 18, NumericScale = 0, IsNullable = true },
        ];

        // Act.
        dbConnection
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo)
            .CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                NumericPrecision = x.NumericPrecision,
                NumericScale = x.NumericScale,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public override void GetColumns_Real()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
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
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, CharMaxLength = null, NumericPrecision = 32, NumericScale = 0, IsPrimaryKey = true, IsNullable = false },
            new() { ColumnName = "value1", CalculatedDbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value2", CalculatedDbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value3", CalculatedDbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value4", CalculatedDbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value5", CalculatedDbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value6", CalculatedDbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
            new() { ColumnName = "value7", CalculatedDbType = DbType.Double, CharMaxLength = null, NumericPrecision = null, NumericScale = null, IsNullable = true },
        ];

        // Act.
        dbConnection
            .GetColumns(tableName, out List<VelocipedeColumnInfo>? columnInfo)
            .CloseDb();
        List<TestColumnInfo> result = columnInfo
            .Select(x => new TestColumnInfo
            {
                ColumnName = x.ColumnName,
                CalculatedDbType = x.CalculatedDbType,
                CharMaxLength = x.CharMaxLength,
                NumericPrecision = x.NumericPrecision,
                NumericScale = x.NumericScale,
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    public override void GetColumns_Text()
    {
        throw new NotImplementedException();
    }
}
