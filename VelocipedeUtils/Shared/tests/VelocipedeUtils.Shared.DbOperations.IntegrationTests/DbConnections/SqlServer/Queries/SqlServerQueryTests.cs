using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Queries;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerQueryTests : BaseQueryTests
{
    public SqlServerQueryTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
