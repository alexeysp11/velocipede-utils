using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Connecting;

public sealed class SqliteDbExistsTests : BaseDbExistsTests, IClassFixture<SqliteDatabaseFixture>
{
    public SqliteDbExistsTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
    {
    }
}