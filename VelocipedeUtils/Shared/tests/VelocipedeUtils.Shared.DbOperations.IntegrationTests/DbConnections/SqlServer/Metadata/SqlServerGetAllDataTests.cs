using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.TestInfrastructure.DatabaseFixtures.ReusableCollections;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.SqlServer.Metadata;

[Collection(nameof(SqlServerDatabaseFixtureCollection))]
public sealed class SqlServerGetAllDataTests : BaseGetAllDataTests
{
    /// <summary>
    /// Default constructor for creating <see cref="SqlServerGetAllDataTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public SqlServerGetAllDataTests(SqlServerDatabaseFixture fixture)
        : base(fixture, SqlServerTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
