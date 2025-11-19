using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.Metadata;

public sealed class PgGetAllDataTests : BaseGetAllDataTests, IClassFixture<PgDatabaseFixture>
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
