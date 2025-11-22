using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.Constants;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DatabaseFixtures;
using VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base.Metadata;
using VelocipedeUtils.Shared.DbOperations.Models;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Sqlite.Metadata;

/// <summary>
/// Class for testing <see cref="SqliteDbConnection.GetColumns(string, out List{VelocipedeColumnInfo})"/>.
/// </summary>
public sealed class SqliteGetColumnsTests : BaseGetColumnsTests, IClassFixture<SqliteDatabaseFixture>
{
    /// <summary>
    /// Default constructor for creating <see cref="SqliteGetColumnsTests"/>.
    /// </summary>
    /// <param name="fixture">Database fixture.</param>
    public SqliteGetColumnsTests(SqliteDatabaseFixture fixture)
        : base(fixture, SqliteTestConstants.CREATE_DATABASE_SQL)
    {
    }
}
