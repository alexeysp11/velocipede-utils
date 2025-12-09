using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Metadata;

[Collection(nameof(PgDatabaseFixtureCollection))]
public sealed class PgGetAllDataTests : BaseGetAllDataTests
{
    /// <summary>
    /// Default constructor for creating <see cref="PgGetAllDataTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public PgGetAllDataTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
