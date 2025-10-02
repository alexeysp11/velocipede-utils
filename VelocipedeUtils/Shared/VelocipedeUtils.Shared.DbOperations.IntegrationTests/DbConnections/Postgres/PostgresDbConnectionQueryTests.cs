using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<PgDatabaseFixture>
    {
        public PostgresDbConnectionQueryTests(PgDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
