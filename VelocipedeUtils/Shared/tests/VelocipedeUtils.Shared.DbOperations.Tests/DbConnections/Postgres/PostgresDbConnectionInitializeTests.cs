using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public PostgresDbConnectionInitializeTests() : base(DatabaseType.PostgreSQL)
        {
            _connectionString = "Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase;";
        }
    }
}
