using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections
{
    public sealed class PostgresDbConnectionTests : BaseDbConnectionTests, IClassFixture<PgDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
create table if not exists ""TestModels"" (
    ""Id"" int primary key NOT NULL,
    ""Name"" varchar(50) NOT NULL,
    ""AdditionalInfo"" varchar(50) NULL);

create table if not exists ""TestCities"" (
    ""Id"" int primary key NOT NULL,
    ""Name"" varchar(50) NOT NULL);

create table if not exists ""TestUsers"" (
    ""Id"" int primary key NOT NULL,
    ""Name"" varchar(50) NOT NULL,
    ""Email"" varchar(50) NOT NULL,
    ""CityId"" int NULL REFERENCES ""TestCities""(""Id""),
    ""AdditionalInfo"" varchar(50) NULL);";

        public PostgresDbConnectionTests(PgDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}
