using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<MssqlDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
create table if not exists ""TestModels"" (
    ""Id"" int primary key,
    ""Name"" varchar(50),
    ""AdditionalInfo"" varchar(50)
);";

        public MssqlDbConnectionQueryTests(MssqlDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}
