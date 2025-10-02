using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionInitializeTests : BaseDbConnectionInitializeTests, IClassFixture<MssqlDatabaseFixture>
    {
        public MssqlDbConnectionInitializeTests(MssqlDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
