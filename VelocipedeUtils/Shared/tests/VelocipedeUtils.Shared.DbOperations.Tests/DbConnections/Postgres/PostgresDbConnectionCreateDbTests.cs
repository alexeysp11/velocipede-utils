using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
    {
        public PostgresDbConnectionCreateDbTests() : base(Enums.DatabaseType.PostgreSQL)
        {
        }
    }
}
