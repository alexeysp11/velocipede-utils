using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Connecting;

[Collection(nameof(PgDatabaseFixtureCollection))]
public sealed class PgOpenDbTests : BaseOpenDbTests
{
    public PgOpenDbTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
