using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
    {
        public MysqlDbConnectionDbExistsTests() : base(Enums.DatabaseType.MySQL)
        {
        }
    }
}
