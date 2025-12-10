using System.Data;
using FluentAssertions;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
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
[Collection("DatabaseFixtureResolverCollection")]
public sealed class CreateTableQueryBuilderTests
{
    private const string COLUMN_NAME = "ColumnName1";

    private readonly DatabaseFixtureResolver _fixtureResolver;

    public CreateTableQueryBuilderTests(DatabaseFixtureResolver fixtureResolver)
    {
        _fixtureResolver = fixtureResolver;
    }

    public static TheoryData<TestCaseNativeColumnType> GetColumnInfoReal()
    {
        return new TheoryData<TestCaseNativeColumnType>
        {
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Double }, ExpectedNativeType = "double precision" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.SQLite, ColumnType = DbType.Single }, ExpectedNativeType = "double precision" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Double }, ExpectedNativeType = "double precision" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.PostgreSQL, ColumnType = DbType.Single }, ExpectedNativeType = "double precision" } },

            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Double }, ExpectedNativeType = "double precision" } },
            { new() { ColumnInfo = new() { ColumnName = COLUMN_NAME, DatabaseType = VelocipedeDatabaseType.MSSQL, ColumnType = DbType.Single }, ExpectedNativeType = "double precision" } },
        };
    }

    [Theory]
    [MemberData(nameof(GetColumnInfoReal))]
    public async Task Builder_GeneratesCorrectNativeType(TestCaseNativeColumnType testCase)
    {
        // Assert.
        string tableName = Guid.NewGuid().ToString();
        VelocipedeDatabaseType databaseType = testCase.ColumnInfo.DatabaseType;
        IDatabaseFixture fixture = _fixtureResolver.GetFixture(databaseType);

        // Act.
        // 1. Get create table statement using query builder.
        CreateTableQueryBuilder queryBuilder = new(databaseType, tableName);
        string? createSql = queryBuilder
            .WithColumn(testCase.ColumnInfo)
            .Build()
            .ToString();

        // 2. Use database connection to create table and get the metadata.
        IVelocipedeDbConnection dbConnection = fixture.GetVelocipedeDbConnection();
#nullable disable
        await dbConnection.ExecuteAsync(createSql);
#nullable restore
        List<VelocipedeNativeColumnInfo> resultColumns = await dbConnection.GetColumnsAsync(tableName);

        // Assert.
        createSql
            .Should()
            .NotBeNullOrEmpty();
        resultColumns
            .Should()
            .NotBeNull();
    }
}
