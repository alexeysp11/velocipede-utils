using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Queries;

public sealed class SqliteQueryTests : BaseQueryTests, IClassFixture<SqliteDatabaseFixture>
{
    public SqliteQueryTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
