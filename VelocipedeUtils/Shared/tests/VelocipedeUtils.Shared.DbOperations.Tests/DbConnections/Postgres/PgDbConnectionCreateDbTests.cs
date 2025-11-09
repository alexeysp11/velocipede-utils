using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres;

public sealed class PgDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PgDbConnectionCreateDbTests() : base(Enums.DatabaseType.PostgreSQL)
    {
    }
}
