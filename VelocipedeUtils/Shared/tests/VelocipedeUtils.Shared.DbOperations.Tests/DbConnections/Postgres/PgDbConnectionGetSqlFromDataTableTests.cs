using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres;

public sealed class PgDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
{
    public PgDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.PostgreSQL)
    {
    }
}
