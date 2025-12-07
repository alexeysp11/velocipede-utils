using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres;

public sealed class PgDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PgDbConnectionGetSqlFromDataTableTests() : base(Enums.VelocipedeDatabaseType.PostgreSQL)
    {
    }
}
