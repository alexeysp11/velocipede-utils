using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<MysqlDatabaseFixture>
    {
        public MysqlDbConnectionQueryTests(MysqlDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
