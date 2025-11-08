using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql;

public sealed class MssqlDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
{
    public MssqlDbConnectionGetSqlFromDataTableTests() : base(Enums.DatabaseType.MSSQL)
    {
    }
}
