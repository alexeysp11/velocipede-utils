using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Metadata;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerGetTablesInDbTests : BaseGetTablesInDbTests
{
    public SqlServerGetTablesInDbTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
