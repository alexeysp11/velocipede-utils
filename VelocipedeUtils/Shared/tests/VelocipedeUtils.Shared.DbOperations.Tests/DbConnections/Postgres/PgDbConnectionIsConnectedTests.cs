using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres;

public sealed class PgDbConnectionIsConnectedTests : BaseDbConnectionIsConnectedTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public PgDbConnectionIsConnectedTests() : base(VelocipedeDatabaseType.PostgreSQL)
    {
        _connectionString = "Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase;";
    }
}
