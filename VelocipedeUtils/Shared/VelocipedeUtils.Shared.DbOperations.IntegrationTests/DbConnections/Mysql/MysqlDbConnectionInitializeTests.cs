using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionInitializeTests : BaseDbConnectionInitializeTests, IClassFixture<MysqlDatabaseFixture>
    {
        public MysqlDbConnectionInitializeTests(MysqlDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
