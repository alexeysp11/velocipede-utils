using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mysql
{
    public sealed class OracleDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
    {
        public OracleDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.MySQL)
        {
        }
    }
}
