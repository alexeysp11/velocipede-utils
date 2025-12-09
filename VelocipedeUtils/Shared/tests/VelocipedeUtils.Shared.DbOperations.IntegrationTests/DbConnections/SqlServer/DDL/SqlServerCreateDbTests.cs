using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.DDL;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerCreateDbTests : BaseCreateDbTests
{
    public SqlServerCreateDbTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }

    [Fact(Skip = "This test is unstable due to the login issue in MS SQL when reconnecting multiple times")]
    public override void CreateDb_ConnectAndSetNotExistingDbUsingSetters()
    {
    }

    [Fact(Skip = "This test is unstable due to the login issue in MS SQL when reconnecting multiple times")]
    public override void CreateDb_CreateNotExistingDbUsingExtensionMethod()
    {
    }
}
