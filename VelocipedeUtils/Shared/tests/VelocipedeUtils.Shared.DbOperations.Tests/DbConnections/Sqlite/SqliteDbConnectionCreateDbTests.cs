using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite;

public sealed class SqliteDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
{
    public SqliteDbConnectionCreateDbTests() : base(Enums.DatabaseType.SQLite)
    {
    }
}
