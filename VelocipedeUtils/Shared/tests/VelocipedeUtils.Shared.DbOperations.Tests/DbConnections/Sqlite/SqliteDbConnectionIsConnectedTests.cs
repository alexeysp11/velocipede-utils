using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite;

public sealed class SqliteDbConnectionIsConnectedTests : BaseDbConnectionIsConnectedTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqliteDbConnectionIsConnectedTests() : base(DatabaseType.SQLite)
    {
        _connectionString = "Data Source=SqliteDbConnectionIsConnectedTests/SqliteDbConnectionIsConnectedTests.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;";
    }
}
