using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Mssql.Metadata;

[Collection(nameof(MssqlDatabaseFixtureCollection))]
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
