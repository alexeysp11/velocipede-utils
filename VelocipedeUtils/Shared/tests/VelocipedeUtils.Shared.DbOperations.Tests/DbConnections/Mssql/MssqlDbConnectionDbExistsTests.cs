using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
    {
        public MssqlDbConnectionDbExistsTests() : base(Enums.DatabaseType.MSSQL)
        {
        }
    }
}
