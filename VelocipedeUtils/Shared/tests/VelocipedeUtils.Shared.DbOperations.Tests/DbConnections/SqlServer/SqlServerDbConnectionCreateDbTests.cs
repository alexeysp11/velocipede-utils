using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.SqlServer;

public sealed class SqlServerDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlServerDbConnectionCreateDbTests() : base(Enums.VelocipedeDatabaseType.MSSQL)
    {
    }
}
