using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.DDL;

public sealed class SqliteCreateDbIfNotExistsTests : BaseCreateDbIfNotExistsTests, IClassFixture<SqliteDatabaseFixture>
{
    public SqliteCreateDbIfNotExistsTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
