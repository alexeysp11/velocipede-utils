using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections;

/// <summary>
/// Unit tests for <see cref="PgDbConnection"/>.
/// </summary>
public sealed class PgDbConnectionTests : BaseDbConnectionTests, IClassFixture<PgDatabaseFixture>
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

CREATE OR REPLACE FUNCTION ""fn_TestUsers_CityId_Update""()
RETURNS TRIGGER AS $$
BEGIN
    update ""TestUsers""
    set ""AdditionalInfo"" = 'No city specified'
    where ""Id"" = new.""Id"" and new.""CityId"" is null;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

create or replace trigger ""trg_AfterUpdate_TestUsers_CityId"" after update of ""CityId"" on ""TestUsers""
execute function ""fn_TestUsers_CityId_Update""();

create or replace trigger ""trg_Insert_TestUsers_CityId"" after insert on ""TestUsers""
execute function ""fn_TestUsers_CityId_Update""();";

    private const string CREATE_TESTMODELS_SQL = @"CREATE TABLE public.TestModels
(
    Id integer, 
    Name character varying(50), 
    AdditionalInfo character varying(50)
);";

    private const string CREATE_TESTUSERS_SQL = @"CREATE TABLE public.TestUsers
(
    Id integer, 
    Name character varying(50), 
    Email character varying(50), 
    CityId integer, 
    AdditionalInfo character varying(50)
);";

    public PgDbConnectionTests(PgDatabaseFixture fixture)
        : base(fixture, CREATE_DATABASE_SQL, CREATE_TESTMODELS_SQL, CREATE_TESTUSERS_SQL)
    {
        // Queries for Execute.
        _createTableSqlForExecuteQuery = @"create table if not exists ""TestTableForExecute"" (""Name"" varchar(50) NOT NULL)";
        _createTableSqlForExecuteWithParamsQuery = @"
create table if not exists ""TestTableForExecuteWithParams"" (""Name"" varchar(50) NOT NULL);
insert into ""TestTableForExecuteWithParams"" (""Name"") values (@TestRecordName);";

        // Queries for ExecuteAsync.
        _createTableSqlForExecuteAsyncQuery = @"create table if not exists ""TestTableForExecuteAsync"" (""Name"" varchar(50) NOT NULL)";
        _createTableSqlForExecuteAsyncWithParamsQuery = @"
create table if not exists ""TestTableForExecuteAsyncWithParams"" (""Name"" varchar(50) NOT NULL);
insert into ""TestTableForExecuteAsyncWithParams"" (""Name"") values (@TestRecordName);";
    }
}
