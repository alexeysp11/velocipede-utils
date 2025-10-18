using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite
{
    public sealed class SqliteDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
    {
        public SqliteDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.SQLite)
        {
        }
    }
}
