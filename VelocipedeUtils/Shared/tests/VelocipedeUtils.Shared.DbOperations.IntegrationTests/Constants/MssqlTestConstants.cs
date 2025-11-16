namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;

public sealed class MssqlTestConstants
{
    public const string CREATE_DATABASE_SQL = @"
DECLARE @triggerSql NVARCHAR(MAX);

IF OBJECT_ID(N'dbo.""TestModels""', N'U') IS NULL
BEGIN
    create table dbo.""TestModels"" (
        ""Id"" int primary key,
        ""Name"" varchar(50) NOT NULL,
        ""AdditionalInfo"" varchar(50) NULL);
END;

IF OBJECT_ID(N'dbo.""TestCities""', N'U') IS NULL
BEGIN
    create table dbo.""TestCities"" (
        ""Id"" int primary key,
        ""Name"" varchar(50) NOT NULL);
END;

IF OBJECT_ID(N'dbo.""TestUsers""', N'U') IS NULL
BEGIN
    create table dbo.""TestUsers"" (
        ""Id"" int primary key,
        ""Name"" varchar(50) NOT NULL,
        ""Email"" varchar(50) NOT NULL,
        ""CityId"" int NULL,
        ""AdditionalInfo"" varchar(50) NULL,
        FOREIGN KEY (""CityId"") REFERENCES ""TestCities""(""Id""));
END;

SET @triggerSql = 'create or alter trigger ""trg_AfterUpdate_TestUsers_CityId"" on ""TestUsers"" after update as begin update ""TestUsers"" set ""AdditionalInfo"" = ''No city specified'' from inserted where ""TestUsers"".""Id"" = inserted.""Id"" and inserted.""CityId"" is null; end;';
EXEC sp_executesql @triggerSql;

SET @triggerSql = 'create or alter trigger ""trg_Insert_TestUsers_CityId"" on ""TestUsers"" after insert as begin update ""TestUsers"" set ""AdditionalInfo"" = ''No city specified'' from inserted where ""TestUsers"".""Id"" = inserted.""Id"" and inserted.""CityId"" is null; end;';
EXEC sp_executesql @triggerSql;";

    public const string CREATE_TESTMODELS_SQL = @"CREATE TABLE dbo.TestModels
(
    Id integer, 
    Name character varying(50), 
    AdditionalInfo character varying(50)
);";

    public const string CREATE_TESTUSERS_SQL = @"CREATE TABLE dbo.TestUsers
(
    Id integer, 
    Name character varying(50), 
    Email character varying(50), 
    CityId integer, 
    AdditionalInfo character varying(50)
);";
}
