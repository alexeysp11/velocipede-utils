using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.SqlServer;

public sealed class SqlServerDbConnectionInitializeTests : BaseDbConnectionInitializeTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqlServerDbConnectionInitializeTests() : base(VelocipedeDatabaseType.MSSQL)
    {
        _connectionString = "Data Source=YourServerName;Initial Catalog=YourDatabaseName;User ID=YourUsername;Password=YourPassword;";
    }
}
