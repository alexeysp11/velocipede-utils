using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.SqlServer;

public sealed class SqlServerDbConnectionGetSqlFromDataTableTests : BaseDbConnectionGetSqlFromDataTableTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlServerDbConnectionGetSqlFromDataTableTests() : base(Enums.VelocipedeDatabaseType.MSSQL)
    {
    }
}
