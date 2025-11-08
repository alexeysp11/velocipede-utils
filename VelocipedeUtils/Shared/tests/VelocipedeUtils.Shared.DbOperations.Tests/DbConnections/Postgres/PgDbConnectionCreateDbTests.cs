using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres;

public sealed class PgDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
{
    public PgDbConnectionCreateDbTests() : base(Enums.DatabaseType.PostgreSQL)
    {
    }
}
