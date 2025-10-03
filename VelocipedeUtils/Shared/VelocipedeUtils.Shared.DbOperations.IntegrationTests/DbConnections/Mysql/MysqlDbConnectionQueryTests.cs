using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<MysqlDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
create table if not exists ""TestModels"" (
    ""Id"" int primary key,
    ""Name"" varchar(50),
    ""AdditionalInfo"" varchar(50)
);";

        public MysqlDbConnectionQueryTests(MysqlDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}
