using Dapper;
using Npgsql;

namespace VelocipedeUtils.Ubp.DbInit.Dal
{
    /// <summary>
    /// DAO for initializing postgres database for UnifiedBusinessPlatform.
    /// </summary>
    internal static class PgDatabaseInitDao
    {
        /// <summary>
        /// Check if the database exists.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="sql">SQL query to execute</param>
        /// <returns>true if the database exists; otherwise, false</returns>
        internal static bool CheckDbExists(string connectionString, string sql)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                string? databaseName = connection.ExecuteScalar<string?>(sql);
                return !string.IsNullOrEmpty(databaseName);
            }
        }

        /// <summary>
        /// Execute SQL query.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="sql">SQL query to execute</param>
        internal static void ExecuteSqlQuery(string connectionString, string sql)
        {
            using (var connection = new NpgsqlConnection(connectionString))
            {
                connection.Execute(sql, commandTimeout: 600);
            }
        }
    }
}
