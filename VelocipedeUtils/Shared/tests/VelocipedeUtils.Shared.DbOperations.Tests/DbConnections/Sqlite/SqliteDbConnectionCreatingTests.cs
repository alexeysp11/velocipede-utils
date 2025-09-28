using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Sqlite
{
    public sealed class SqliteDbConnectionCreatingTests : BaseDbConnectionCreatingTests
    {
        public SqliteDbConnectionCreatingTests() : base(Enums.DatabaseType.SQLite)
        {
            _connectionString = "Data Source=mydatabase.db;Mode=ReadWriteCreate;Cache=Shared;Foreign Keys=True;";
        }
    }
}
