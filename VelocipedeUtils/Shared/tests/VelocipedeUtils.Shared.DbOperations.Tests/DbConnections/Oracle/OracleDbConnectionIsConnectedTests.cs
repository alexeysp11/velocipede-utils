using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Oracle
{
    public sealed class OracleDbConnectionIsConnectedTests : BaseDbConnectionIsConnectedTests
    {
        public OracleDbConnectionIsConnectedTests() : base(DatabaseType.Oracle)
        {
            _connectionString = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=MyHost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=MyService)));User ID=myusername;Password=mypassword;";
        }
    }
}
