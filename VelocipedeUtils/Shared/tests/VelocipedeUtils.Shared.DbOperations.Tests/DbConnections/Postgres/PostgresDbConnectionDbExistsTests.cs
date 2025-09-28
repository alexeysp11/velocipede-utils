using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
    {
        public PostgresDbConnectionDbExistsTests() : base(Enums.DatabaseType.PostgreSQL)
        {
        }
    }
}
