using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Connecting;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerOpenDbTests : BaseOpenDbTests
{
    public SqlServerOpenDbTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
