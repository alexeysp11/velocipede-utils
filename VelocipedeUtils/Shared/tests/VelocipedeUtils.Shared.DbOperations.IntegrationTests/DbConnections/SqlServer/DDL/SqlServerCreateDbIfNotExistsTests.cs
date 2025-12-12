using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.DDL;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerCreateDbIfNotExistsTests : BaseCreateDbIfNotExistsTests
{
    public SqlServerCreateDbIfNotExistsTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
