using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
    {
        public PostgresDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.PostgreSQL)
        {
        }
    }
}
