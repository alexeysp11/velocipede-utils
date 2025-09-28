using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
    {
        public MysqlDbConnectionCreateDbTests() : base(Enums.DatabaseType.MySQL)
        {
        }
    }
}
