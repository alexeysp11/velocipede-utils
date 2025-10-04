using System;
using VelocipedeUtils.Shared.DbOperations.DbConnections;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.Factories
{
    /// <summary>
    /// Class for creating database connections.
    /// </summary>
    public static class VelocipedeDbConnectionFactory
    {
        /// <summary>
        /// Initialize database connection.
        /// </summary>
        public static IVelocipedeDbConnection InitializeDbConnection(DatabaseType databaseType, string connectionString = null)
        {
            return databaseType switch
            {
                DatabaseType.SQLite => new SqliteDbConnection(connectionString),
                DatabaseType.PostgreSQL => new PgDbConnection(connectionString),
                DatabaseType.MSSQL => new MssqlDbConnection(connectionString),
                DatabaseType.MySQL => throw new NotImplementedException("This class is corrently excluded due to .NET MySQL errors \"The given key was not present in the dictionary\""),
                DatabaseType.Oracle => new OracleDbConnection(connectionString),
                _ => throw new ArgumentException("Specified database type is not valid", nameof(databaseType))
            };
        }
    }
}
