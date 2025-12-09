using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.SqlServer;

public sealed class SqlServerDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlServerDbConnectionDbExistsTests() : base(Enums.VelocipedeDatabaseType.MSSQL)
    {
    }
}
