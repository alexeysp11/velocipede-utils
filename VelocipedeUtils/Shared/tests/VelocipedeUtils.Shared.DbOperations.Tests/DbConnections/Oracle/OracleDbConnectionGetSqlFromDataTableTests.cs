using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Oracle
{
    public sealed class PostgresDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
    {
        public PostgresDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.Oracle)
        {
        }
    }
}
