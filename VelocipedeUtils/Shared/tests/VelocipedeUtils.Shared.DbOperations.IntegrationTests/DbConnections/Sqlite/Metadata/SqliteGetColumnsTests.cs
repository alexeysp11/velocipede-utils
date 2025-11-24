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
    value25 Int8
)";
        dbConnection
            .OpenDb()
            .BeginTransaction()
            .Execute(sql)
            .CommitTransaction();

        // 3. Expected result.
        //List<TestColumnInfo> expected = _expectedCaseInsensitiveColumnInfos;
        int expectedQty = 26;

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
        //result.Should().BeEquivalentTo(expected);
        result.Should().HaveCount(expectedQty);
    }

    public override void GetColumns_NumericAffinity()
    {
        throw new NotImplementedException();
    }

    public override void GetColumns_RealAffinity()
    {
        throw new NotImplementedException();
    }

    public override void GetColumns_TextAffinity()
    {
        throw new NotImplementedException();
    }
}
