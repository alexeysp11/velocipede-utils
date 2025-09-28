﻿using VelocipedeUtils.Shared.DbOperations.Enums;
using VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Base;

namespace VelocipedeUtils.Shared.DbOperations.Tests.DbConnections.Mssql
{
    public sealed class MssqlDbConnectionInitializeTests : BaseDbConnectionInitializeTests
    {
        public MssqlDbConnectionInitializeTests() : base(DatabaseType.MSSQL)
        {
            _connectionString = "Data Source=YourServerName;Initial Catalog=YourDatabaseName;User ID=YourUsername;Password=YourPassword;";
        }
    }
}
