using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Postgres.DDL;

public sealed class PgCreateDbIfNotExistsTests : BaseCreateDbIfNotExistsTests, IClassFixture<PgDatabaseFixture>
{
    public PgCreateDbIfNotExistsTests(PgDatabaseFixture fixture)
        : base(fixture, PgTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
