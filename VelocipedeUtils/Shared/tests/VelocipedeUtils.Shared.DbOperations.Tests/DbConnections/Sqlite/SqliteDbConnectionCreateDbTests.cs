using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite;

public sealed class SqliteDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqliteDbConnectionCreateDbTests() : base(Enums.VelocipedeDatabaseType.SQLite)
    {
    }
}
