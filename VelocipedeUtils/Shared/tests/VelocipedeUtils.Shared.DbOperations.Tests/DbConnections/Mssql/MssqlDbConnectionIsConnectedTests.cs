using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionIsConnectedTests : BaseDbConnectionIsConnectedTests
    {
        public MssqlDbConnectionIsConnectedTests() : base(DatabaseType.MSSQL)
        {
            _connectionString = "Data Source=YourServerName;Initial Catalog=YourDatabaseName;User ID=YourUsername;Password=YourPassword;";
        }
    }
}
