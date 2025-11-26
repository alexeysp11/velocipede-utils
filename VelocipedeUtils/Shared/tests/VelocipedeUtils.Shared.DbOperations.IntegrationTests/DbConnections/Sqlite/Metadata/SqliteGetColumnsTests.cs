using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Metadata;

/// <summary>
/// Class for testing <see cref="SqliteDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
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

    public override void GetColumns_BlobAffinity()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public override void GetColumns_IntegerAffinity()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = nameof(GetColumns_IntegerAffinity);
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
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value2", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value3", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value4", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value5", CalculatedDbType = DbType.SByte, IsNullable = true },
            new() { ColumnName = "value6", CalculatedDbType = DbType.SByte, IsNullable = true },
            new() { ColumnName = "value7", CalculatedDbType = DbType.SByte, IsNullable = true },
            new() { ColumnName = "value8", CalculatedDbType = DbType.Int16, IsNullable = true },
            new() { ColumnName = "value9", CalculatedDbType = DbType.Int16, IsNullable = true },
            new() { ColumnName = "value10", CalculatedDbType = DbType.Int16, IsNullable = true },
            new() { ColumnName = "value11", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value12", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value13", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value14", CalculatedDbType = DbType.Int64, IsNullable = true },
            new() { ColumnName = "value15", CalculatedDbType = DbType.Int64, IsNullable = true },
            new() { ColumnName = "value16", CalculatedDbType = DbType.Int64, IsNullable = true },
            new() { ColumnName = "value17", CalculatedDbType = DbType.UInt64, IsNullable = true },
            new() { ColumnName = "value18", CalculatedDbType = DbType.UInt64, IsNullable = true },
            new() { ColumnName = "value19", CalculatedDbType = DbType.UInt64, IsNullable = true },
            new() { ColumnName = "value20", CalculatedDbType = DbType.Int16, IsNullable = true },
            new() { ColumnName = "value21", CalculatedDbType = DbType.Int16, IsNullable = true },
            new() { ColumnName = "value22", CalculatedDbType = DbType.Int16, IsNullable = true },
            new() { ColumnName = "value23", CalculatedDbType = DbType.Int64, IsNullable = true },
            new() { ColumnName = "value24", CalculatedDbType = DbType.Int64, IsNullable = true },
            new() { ColumnName = "value25", CalculatedDbType = DbType.Int64, IsNullable = true },
            new() { ColumnName = "value26", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value27", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value28", CalculatedDbType = DbType.Int32, IsNullable = true },
            new() { ColumnName = "value29", CalculatedDbType = DbType.UInt64, IsNullable = true },
            new() { ColumnName = "value30", CalculatedDbType = DbType.UInt64, IsNullable = true },
            new() { ColumnName = "value31", CalculatedDbType = DbType.UInt64, IsNullable = true },
            new() { ColumnName = "value32", CalculatedDbType = DbType.UInt32, IsNullable = true },
            new() { ColumnName = "value33", CalculatedDbType = DbType.UInt32, IsNullable = true },
            new() { ColumnName = "value34", CalculatedDbType = DbType.UInt32, IsNullable = true },
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
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }

    public override void GetColumns_NumericAffinity()
    {
        throw new NotImplementedException();
    }

    public override void GetColumns_RealAffinity()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public override void GetColumns_TextAffinity()
    {
        // Arrange.
        // 1. Database connection.
        using IVelocipedeDbConnection dbConnection = _fixture.GetVelocipedeDbConnection();

        // 2. Create table.
        string tableName = nameof(GetColumns_IntegerAffinity);
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
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        List<TestColumnInfo> expected =
        [
            new() { ColumnName = "id", CalculatedDbType = DbType.Int32, IsPrimaryKey = true, IsNullable = true },
            new() { ColumnName = "value1", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value2", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value3", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value4", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value5", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value6", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value7", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value8", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value9", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value10", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value11", CalculatedDbType = DbType.String, CharMaxLength = 55, IsNullable = true },
            new() { ColumnName = "value12", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value13", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value14", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value15", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value16", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value17", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value18", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
            new() { ColumnName = "value19", CalculatedDbType = DbType.String, CharMaxLength = -1, IsNullable = true },
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
                IsPrimaryKey = x.IsPrimaryKey,
                IsNullable = x.IsNullable,
            })
            .ToList();

        // Assert.
        dbConnection.IsConnected.Should().BeFalse();
        result.Should().BeEquivalentTo(expected);
    }
}
