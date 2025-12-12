using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite;

public sealed class SqliteDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqliteDbConnectionGetSqlFromDataTableTests() : base(Enums.VelocipedeDatabaseType.SQLite)
    {
    }
}
