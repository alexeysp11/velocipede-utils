using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Queries;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Queries;

[Collection(nameof(PgDatabaseFixtureCollection))]
public sealed class PgQueryDataTableTests : BaseQueryDataTableTests
{
    public PgQueryDataTableTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
