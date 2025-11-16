using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Queries;

public sealed class PgExecuteTests : BaseExecuteTests
{
    public PgExecuteTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
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
