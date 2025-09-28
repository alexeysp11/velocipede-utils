using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mysql
{
    public sealed class MysqlDbConnectionCreatingTests : BaseDbConnectionCreatingTests
    {
        public MysqlDbConnectionCreatingTests() : base(Enums.DatabaseType.MySQL)
        {
            _connectionString = "Server=myServerAddress;Database=myDataBase;Uid=myUsername;Pwd=myPassword;";
        }
    }
}
