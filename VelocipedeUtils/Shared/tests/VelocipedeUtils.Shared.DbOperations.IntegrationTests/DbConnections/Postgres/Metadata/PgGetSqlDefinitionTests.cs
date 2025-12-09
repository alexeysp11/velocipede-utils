using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Metadata;

[Collection(nameof(PgDatabaseFixtureCollection))]
public sealed class PgGetSqlDefinitionTests : BaseGetSqlDefinitionTests
{
    public PgGetSqlDefinitionTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL, PgTestConstants.CREATE_TESTMODELS_SQL, PgTestConstants.CREATE_TESTUSERS_SQL)
    {
    }
}
