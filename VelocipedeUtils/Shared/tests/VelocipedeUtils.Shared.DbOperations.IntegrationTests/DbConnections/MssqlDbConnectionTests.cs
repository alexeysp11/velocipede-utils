using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections
{
    /// <summary>
    /// Unit tests for <see cref="MssqlDbConnection"/>.
    /// </summary>
    public sealed class MssqlDbConnectionTests : BaseDbConnectionTests, IClassFixture<MssqlDatabaseFixture>
    {
        private const string CREATE_DATABASE_SQL = @"
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

        private const string CREATE_TESTMODELS_SQL = @"CREATE TABLE dbo.TestModels
(
    Id integer, 
    Name character varying(50), 
    AdditionalInfo character varying(50)
);";

        private const string CREATE_TESTUSERS_SQL = @"CREATE TABLE dbo.TestUsers
(
    Id integer, 
    Name character varying(50), 
    Email character varying(50), 
    CityId integer, 
    AdditionalInfo character varying(50)
);";

        public MssqlDbConnectionTests(MssqlDatabaseFixture fixture)
            : base(fixture, CREATE_DATABASE_SQL, CREATE_TESTMODELS_SQL, CREATE_TESTUSERS_SQL)
        {
            // Queries for Execute.
            _createTableSqlForExecuteQuery = @"create table ""TestTableForExecute"" (""Name"" varchar(50) NOT NULL)";
            _createTableSqlForExecuteWithParamsQuery = @"
create table ""TestTableForExecuteWithParams"" (""Name"" varchar(50) NOT NULL);
insert into ""TestTableForExecuteWithParams"" (""Name"") values (@TestRecordName);";

            // Queries for ExecuteAsync.
            _createTableSqlForExecuteAsyncQuery = @"create table ""TestTableForExecuteAsync"" (""Name"" varchar(50) NOT NULL)";
            _createTableSqlForExecuteAsyncWithParamsQuery = @"
create table ""TestTableForExecuteAsyncWithParams"" (""Name"" varchar(50) NOT NULL);
insert into ""TestTableForExecuteAsyncWithParams"" (""Name"") values (@TestRecordName);";
        }

        [Fact(Skip = "This test is unstable due to the login issue in MS SQL when reconnecting multiple times")]
        public override void CreateDb_ConnectAndSetNotExistingDbUsingSetters_DbExists()
        {
        }

        [Fact(Skip = "This test is unstable due to the login issue in MS SQL when reconnecting multiple times")]
        public override void CreateDb_CreateNotExistingDbUsingExtensionMethod_DbExists()
        {
        }

        [Theory(Skip = "This test is not implemented because you cannot use OBJECT_DEFINITION() with User Tables (type 'U')")]
        [InlineData("\"TestModels\"")]
        [InlineData("\"TestUsers\"")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public override void GetSqlDefinition_ConnectionStringFromFixtureAndNotConnected_ResultEqualsToExpected(string tableName)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
        }

        [Theory(Skip = "This test is not implemented because you cannot use OBJECT_DEFINITION() with User Tables (type 'U')")]
        [InlineData("\"TestModels\"")]
        [InlineData("\"TestUsers\"")]
#pragma warning disable xUnit1026 // Theory methods should use all of their parameters
        public override void GetSqlDefinition_ConnectionStringFromFixtureAndConnected_ResultEqualsToExpected(string tableName)
#pragma warning restore xUnit1026 // Theory methods should use all of their parameters
        {
        }
    }
}
