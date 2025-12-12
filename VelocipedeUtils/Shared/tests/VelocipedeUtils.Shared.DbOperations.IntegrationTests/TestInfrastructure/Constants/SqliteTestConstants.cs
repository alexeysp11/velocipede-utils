namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;

public static class SqliteTestConstants
{
    public const string CREATE_DATABASE_SQL = @"
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

    public const string CREATE_TESTMODELS_SQL = @"CREATE TABLE ""TestModels"" (
    ""Id"" int primary key NOT NULL,
    ""Name"" varchar(50) NOT NULL,
    ""AdditionalInfo"" varchar(50) NULL)";

    public const string CREATE_TESTUSERS_SQL = @"CREATE TABLE ""TestUsers"" (
    ""Id"" int primary key NOT NULL,
    ""Name"" varchar(50) NOT NULL,
    ""Email"" varchar(50) NOT NULL,
    ""CityId"" int NULL REFERENCES ""TestCities""(""Id""),
    ""AdditionalInfo"" varchar(50) NULL)";
}
