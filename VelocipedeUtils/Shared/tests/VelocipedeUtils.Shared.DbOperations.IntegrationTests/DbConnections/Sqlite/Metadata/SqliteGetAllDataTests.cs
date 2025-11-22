using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Metadata;

public sealed class SqliteGetAllDataTests : BaseGetAllDataTests, IClassFixture<SqliteDatabaseFixture>
{
    /// <summary>
    /// Default constructor for creating <see cref="SqliteGetAllDataTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public SqliteGetAllDataTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
