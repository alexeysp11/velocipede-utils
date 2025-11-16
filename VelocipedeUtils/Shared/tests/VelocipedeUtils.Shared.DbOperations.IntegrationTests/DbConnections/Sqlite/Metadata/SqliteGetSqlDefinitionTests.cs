using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Metadata;

public sealed class SqliteGetSqlDefinitionTests : BaseGetSqlDefinitionTests, IClassFixture<SqliteDatabaseFixture>
{
    public SqliteGetSqlDefinitionTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL, SqliteTestConstants.CREATE_TESTMODELS_SQL, SqliteTestConstants.CREATE_TESTUSERS_SQL)
    {
    }
}
