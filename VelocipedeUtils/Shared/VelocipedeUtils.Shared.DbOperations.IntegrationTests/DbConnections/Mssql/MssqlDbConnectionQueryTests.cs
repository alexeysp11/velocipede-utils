using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<MssqlDatabaseFixture>
    {
        public MssqlDbConnectionQueryTests(MssqlDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
