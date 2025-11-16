using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Metadata;

public sealed class MssqlGetAllDataTests : BaseGetAllDataTests, IClassFixture<MssqlDatabaseFixture>
{
    public MssqlGetAllDataTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
