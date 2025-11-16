using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Connecting;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Connecting;

public sealed class MssqlCloseDbTests : BaseCloseDbTests, IClassFixture<MssqlDatabaseFixture>
{
    public MssqlCloseDbTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
