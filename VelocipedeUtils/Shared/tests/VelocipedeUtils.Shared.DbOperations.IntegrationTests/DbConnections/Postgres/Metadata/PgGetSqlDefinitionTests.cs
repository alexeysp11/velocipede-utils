using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Metadata;

public sealed class PgGetSqlDefinitionTests : BaseGetSqlDefinitionTests, IClassFixture<PgDatabaseFixture>
{
    public PgGetSqlDefinitionTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL, PgTestConstants.CREATE_TESTMODELS_SQL, PgTestConstants.CREATE_TESTUSERS_SQL)
    {
    }
}
