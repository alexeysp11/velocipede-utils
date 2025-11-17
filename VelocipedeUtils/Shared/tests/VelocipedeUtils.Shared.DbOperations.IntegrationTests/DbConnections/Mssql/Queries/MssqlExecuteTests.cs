using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Queries;

[Collection("MssqlDatabaseFixtureCollection")]
public sealed class MssqlExecuteTests : BaseExecuteTests
{
    public MssqlExecuteTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
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
}
