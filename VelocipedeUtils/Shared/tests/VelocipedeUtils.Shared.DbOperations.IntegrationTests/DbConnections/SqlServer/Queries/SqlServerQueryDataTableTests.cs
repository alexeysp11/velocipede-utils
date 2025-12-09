using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Queries;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerQueryDataTableTests : BaseQueryDataTableTests
{
    public SqlServerQueryDataTableTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
