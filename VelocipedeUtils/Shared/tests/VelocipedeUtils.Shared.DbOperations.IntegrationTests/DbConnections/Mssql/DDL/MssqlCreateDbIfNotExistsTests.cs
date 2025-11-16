using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.DDL;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.DDL;

public sealed class MssqlCreateDbIfNotExistsTests : BaseCreateDbIfNotExistsTests, IClassFixture<MssqlDatabaseFixture>
{
    public MssqlCreateDbIfNotExistsTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
