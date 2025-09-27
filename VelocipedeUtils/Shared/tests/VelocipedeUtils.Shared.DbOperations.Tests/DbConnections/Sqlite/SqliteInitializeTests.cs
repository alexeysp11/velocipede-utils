using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite
{
    public sealed class SqliteInitializeTests : BaseDbConnectionInitializeTests
    {
        public SqliteInitializeTests() : base(DatabaseType.SQLite)
        {
            _connectionString = "Data Source=mydatabase.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;";
        }
    }
}
