using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Oracle
{
    public sealed class OracleDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
    {
        public OracleDbConnectionDbExistsTests() : base(Enums.DatabaseType.Oracle)
        {
        }
    }
}
