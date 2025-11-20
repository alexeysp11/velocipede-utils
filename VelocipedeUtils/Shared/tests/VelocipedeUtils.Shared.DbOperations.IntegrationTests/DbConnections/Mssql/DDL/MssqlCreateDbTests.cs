using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.DDL;

[Collection("MssqlDatabaseFixtureCollection")]
public sealed class MssqlCreateDbTests : BaseCreateDbTests
{
    public MssqlCreateDbTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
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
