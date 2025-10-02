using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.IntegrationTests.DbConnections.Base
{
    public abstract class BaseDbConnectionInitializeTests
    {
        private DatabaseType _databaseType;
        protected string _connectionString;

        protected BaseDbConnectionInitializeTests(DatabaseType databaseType)
        {
            _databaseType = databaseType;
            _connectionString = string.Empty;
        }
    }
}
