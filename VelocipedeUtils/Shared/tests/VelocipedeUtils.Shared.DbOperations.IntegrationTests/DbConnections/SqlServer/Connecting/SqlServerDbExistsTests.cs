using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Connecting;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerDbExistsTests : BaseDbExistsTests
{
    public SqlServerDbExistsTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
