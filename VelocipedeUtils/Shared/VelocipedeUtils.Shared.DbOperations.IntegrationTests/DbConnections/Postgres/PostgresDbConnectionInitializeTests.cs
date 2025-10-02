using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionInitializeTests : BaseDbConnectionInitializeTests, IClassFixture<PgDatabaseFixture>
    {
        public PostgresDbConnectionInitializeTests(PgDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
