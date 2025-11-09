using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql;

public sealed class MssqlDbConnectionDbExistsTests : BaseDbConnectionDbExistsTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public MssqlDbConnectionDbExistsTests() : base(Enums.DatabaseType.MSSQL)
    {
    }
}
