using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql;

public sealed class MssqlDbConnectionCreateDbTests : BaseDbConnectionCreateDbTests
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    public MssqlDbConnectionCreateDbTests() : base(Enums.DatabaseType.MSSQL)
    {
    }
}
