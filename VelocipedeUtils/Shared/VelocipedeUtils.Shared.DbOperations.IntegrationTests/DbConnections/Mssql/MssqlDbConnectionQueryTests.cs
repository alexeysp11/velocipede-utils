using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionQueryTests : BaseDbConnectionQueryTests, IClassFixture<MssqlDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
create table ""TestModels"" (
    ""Id"" int primary key,
    ""Name"" varchar(50) NOT NULL,
    ""AdditionalInfo"" varchar(50) NULL);

create table ""TestCities"" (
    ""Id"" int primary key,
    ""Name"" varchar(50) NOT NULL);

create table ""TestUsers"" (
    ""Id"" int primary key,
    ""Name"" varchar(50) NOT NULL,
    ""Email"" varchar(50) NOT NULL,
    ""CityId"" int NULL,
    ""AdditionalInfo"" varchar(50) NULL,
    FOREIGN KEY (""CityId"") REFERENCES ""TestCities""(""Id""));";

        public MssqlDbConnectionQueryTests(MssqlDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}
