using System;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Class for creating database connections.
    /// </summary>
    public static class DbConnectionsCreator
    {
        /// <summary>
        /// Initialize database connection.
        /// </summary>
        public static ICommonDbConnection InitializeDbConnection(DatabaseType databaseType, string connectionString = null)
        {
            return databaseType switch
            {
                DatabaseType.SQLite => new SqliteDbConnection(connectionString),
                DatabaseType.PostgreSQL => new PgDbConnection(connectionString),
                DatabaseType.MSSQL => new MssqlDbConnection(connectionString),
                DatabaseType.MySQL => new MysqlDbConnection(connectionString),
                DatabaseType.Oracle => new OracleDbConnection(connectionString),
                _ => throw new ArgumentException("Specified database type is not valid", nameof(databaseType))
            };
        }
    }
}
