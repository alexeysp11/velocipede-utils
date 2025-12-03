using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.DDL;

[Collection(nameof(MssqlDatabaseFixtureCollection))]
public sealed class MssqlCreateDbIfNotExistsTests : BaseCreateDbIfNotExistsTests
{
    public MssqlCreateDbIfNotExistsTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
