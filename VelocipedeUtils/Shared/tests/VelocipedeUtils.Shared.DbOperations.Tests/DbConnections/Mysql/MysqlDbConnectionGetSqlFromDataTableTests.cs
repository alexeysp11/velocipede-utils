using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
    {
        public MysqlDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.MySQL)
        {
        }
    }
}
