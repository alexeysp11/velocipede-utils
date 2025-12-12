using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Queries;

public sealed class SqliteExecuteTests : BaseExecuteTests, IClassFixture<SqliteDatabaseFixture>
{
    public SqliteExecuteTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
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
