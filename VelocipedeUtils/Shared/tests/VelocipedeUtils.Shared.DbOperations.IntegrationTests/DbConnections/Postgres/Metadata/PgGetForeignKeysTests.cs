using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Metadata;

[Collection(nameof(PgDatabaseFixtureCollection))]
public sealed class PgGetForeignKeysTests : BaseGetForeignKeysTests
{
    public PgGetForeignKeysTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
