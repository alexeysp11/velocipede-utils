using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Metadata;

[Collection(nameof(MssqlDatabaseFixtureCollection))]
public sealed class MssqlGetTriggersTests : BaseGetTriggersTests
{
    public MssqlGetTriggersTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
