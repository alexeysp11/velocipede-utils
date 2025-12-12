using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres;

public sealed class PgDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PgDbConnectionDbExistsTests() : base(Enums.VelocipedeDatabaseType.PostgreSQL)
    {
    }
}
