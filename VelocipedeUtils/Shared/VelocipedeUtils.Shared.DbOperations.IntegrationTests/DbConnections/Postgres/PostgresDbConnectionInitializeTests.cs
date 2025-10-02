using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;
using VelocipedeUtils.Shared.DbOperations.Tests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionInitializeTests : BaseDbConnectionInitializeTests, IClassFixture<PgDatabaseFixture>
    {
        public PostgresDbConnectionInitializeTests(PgDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
