using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Oracle
{
    public sealed class OracleDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
    {
        public OracleDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.Oracle)
        {
        }
    }
}
