using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionCreatingTests : BaseDbConnectionCreatingTests
    {
        public MssqlDbConnectionCreatingTests() : base(Enums.DatabaseType.MSSQL)
        {
            _connectionString = "Data Source=YourServerName;Initial Catalog=YourDatabaseName;User ID=YourUsername;Password=YourPassword;";
        }
    }
}
