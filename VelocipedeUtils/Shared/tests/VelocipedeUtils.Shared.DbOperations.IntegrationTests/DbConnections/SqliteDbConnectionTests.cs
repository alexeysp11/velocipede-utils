using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections
{
    public sealed class SqliteDbConnectionTests : BaseDbConnectionTests, IClassFixture<SqliteDatabaseFixture>
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
    ""AdditionalInfo"" varchar(50) NULL);

create trigger if not exists ""trg_AfterUpdate_TestUsers_CityId"" after update of ""CityId"" on ""TestUsers""
begin
    update ""TestUsers""
    set ""AdditionalInfo"" = 'No city specified'
    where ""Id"" = new.""Id"" and new.""CityId"" is null;
end;

create trigger if not exists ""trg_Insert_TestUsers_CityId"" after insert on ""TestUsers""
begin
    update ""TestUsers""
    set ""AdditionalInfo"" = 'No city specified'
    where ""Id"" = new.""Id"" and new.""CityId"" is null;
end;";

        public SqliteDbConnectionTests(SqliteDatabaseFixture fixture) : base(fixture, CREATE_DATABASE_SQL)
        {
        }
    }
}
