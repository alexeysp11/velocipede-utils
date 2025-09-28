using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Oracle
{
    public sealed class OracleDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
    {
        public OracleDbConnectionCreateDbTests() : base(Enums.DatabaseType.Oracle)
        {
        }
    }
}
