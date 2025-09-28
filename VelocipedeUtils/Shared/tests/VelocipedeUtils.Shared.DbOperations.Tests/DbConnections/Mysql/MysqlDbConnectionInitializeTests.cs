using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mysql
{
    public class MysqlDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public MysqlDbConnectionInitializeTests() : base(DatabaseType.MySQL)
        {
            _connectionString = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
        }
    }
}
