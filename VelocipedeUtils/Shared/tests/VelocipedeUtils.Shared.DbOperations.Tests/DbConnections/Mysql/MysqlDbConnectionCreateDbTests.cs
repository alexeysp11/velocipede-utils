// This class is corrently excluded due to .NET MySQL errors "The given key was not present in the dictionary".

#define EXCLUDE_MYSQL_PROVIDER

#if !EXCLUDE_MYSQL_PROVIDER


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

#endif
