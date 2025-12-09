using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Metadata;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerGetSqlDefinitionTests : BaseGetSqlDefinitionTests
{
    public SqlServerGetSqlDefinitionTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL, SqlServerTestConstants.CREATE_TESTMODELS_SQL, SqlServerTestConstants.CREATE_TESTUSERS_SQL)
    {
    }

    [Theory(Skip = "This test is not implemented because you cannot use OBJECT_DEFINITION() with User Tables (type 'U')")]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public override void GetSqlDefinition_FixtureNotConnected(string tableName)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
    }

    [Theory(Skip = "This test is not implemented because you cannot use OBJECT_DEFINITION() with User Tables (type 'U')")]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public override void GetSqlDefinition_FixtureConnected(string tableName)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
    }

    [Theory(Skip = "This test is not implemented because you cannot use OBJECT_DEFINITION() with User Tables (type 'U')")]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public override Task GetSqlDefinitionAsync_FixtureNotConnected(string tableName)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
        return Task.CompletedTask;
    }

    [Theory(Skip = "This test is not implemented because you cannot use OBJECT_DEFINITION() with User Tables (type 'U')")]
    [InlineData("\"TestModels\"")]
    [InlineData("\"TestUsers\"")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public override Task GetSqlDefinitionAsync_FixtureConnected(string tableName)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
    {
        return Task.CompletedTask;
    }
}
