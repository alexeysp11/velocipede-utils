using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres;

public sealed class PgDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
{
    public PgDbConnectionDbExistsTests() : base(Enums.DatabaseType.PostgreSQL)
    {
    }
}
