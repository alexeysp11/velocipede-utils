using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Metadata;

[Collection("MssqlDatabaseFixtureCollection")]
public sealed class MssqlGetAllDataTests : BaseGetAllDataTests
{
    /// <summary>
    /// Default constructor for creating <see cref="MssqlGetAllDataTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public MssqlGetAllDataTests(MssqlDatabaseFixture fixture)
        : base(fixture, MssqlTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
