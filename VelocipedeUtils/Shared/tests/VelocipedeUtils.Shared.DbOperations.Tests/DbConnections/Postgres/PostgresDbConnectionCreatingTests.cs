using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Postgres
{
    public sealed class PostgresDbConnectionCreatingTests : BaseDbConnectionCreatingTests
    {
        public PostgresDbConnectionCreatingTests() : base(Enums.DatabaseType.PostgreSQL)
        {
            _connectionString = "Host=localhost;Port=5432;Username=myuser;Password=mypassword;Database=mydatabase;";
        }
    }
}
