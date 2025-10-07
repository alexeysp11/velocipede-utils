using System;
using System.Data;
using System.Text;
using VelocipedeUtils.Shared.DbOperations.Constants;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Extension methods for <see cref="IVelocipedeDbConnection"/>.
    /// </summary>
    public static class IVelocipedeDbConnectionExtensions
    {
        /// <summary>
        /// Set connection string.
        /// </summary>
        public static IVelocipedeDbConnection SetConnectionString(this IVelocipedeDbConnection connection, string connectionString)
        {
            connection.ConnectionString = connectionString;
            return connection;
        }

        /// <summary>
        /// Get database name from the active connection string.
        /// </summary>
        public static string GetActiveDatabaseName(this IVelocipedeDbConnection connection)
        {
            connection.GetActiveDatabaseName(out string dbName);
            return dbName;
        }

        /// <summary>
        /// Get database name from the active connection string.
        /// </summary>
        public static IVelocipedeDbConnection GetActiveDatabaseName(this IVelocipedeDbConnection connection, out string dbName)
        {
            dbName = connection.DatabaseType switch
            {
                DatabaseType.SQLite => SqliteDbConnection.GetDatabaseName(connection.ConnectionString),
                DatabaseType.PostgreSQL => PgDbConnection.GetDatabaseName(connection.ConnectionString),
                DatabaseType.MSSQL => MssqlDbConnection.GetDatabaseName(connection.ConnectionString),
                DatabaseType.MySQL or DatabaseType.MariaDB or DatabaseType.HSQLDB or DatabaseType.Oracle => throw new NotSupportedException(ErrorMessageConstants.DatabaseTypeIsNotSupported),
                _ => throw new InvalidOperationException(ErrorMessageConstants.IncorrectDatabaseType),
            };
            return connection;
        }

        /// <summary>
        /// Create database if not exists.
        /// </summary>
        public static IVelocipedeDbConnection CreateDbIfNotExists(this IVelocipedeDbConnection connection)
        {
            if (!connection.DbExists())
            {
                connection.CreateDb();
            }
            return connection;
        }

        /// <summary>
        /// Switch to the specified database.
        /// </summary>
        public static IVelocipedeDbConnection SwitchDb(this IVelocipedeDbConnection connection, string dbName)
        {
            return connection.SwitchDb(dbName, out _);
        }

        /// <summary>
        /// Get all data from the table.
        /// </summary>
        /// <param name="tableName">Table name</param>
        public static IVelocipedeDbConnection GetAllDataFromTable(this IVelocipedeDbConnection connection, string tableName, out DataTable dtResult)
        {
            string sql = $"SELECT * FROM {tableName}";
            return connection.ExecuteSqlCommand(sql, out dtResult);
        }

        /// <summary>
        /// Execute SQL command without return.
        /// </summary>
        public static IVelocipedeDbConnection ExecuteSqlCommand(this IVelocipedeDbConnection connection, string sql)
        {
            return connection.ExecuteSqlCommand(sql, out _);
        }

        /// <summary>
        /// Gets SQL definition of the DataTable object.
        /// </summary>
        public static string GetSqlFromDataTable(this IVelocipedeDbConnection connection, DataTable dt, string tableName)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt), "Data table could not be null");
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "Table name is not assigned");
            if (dt.Columns.Count == 0)
                throw new ArgumentException("Data table could not be empty");

            var sbSqlRequest = new StringBuilder();
            var sbSqlInsert = new StringBuilder();

            int i = 0;
            sbSqlRequest.Append("CREATE TABLE IF NOT EXISTS " + tableName + " (");
            sbSqlInsert.Append("INSERT INTO " + tableName + " (");
            foreach (DataColumn column in dt.Columns)
            {
                sbSqlRequest.Append(column.ColumnName + " TEXT" + (i != dt.Columns.Count - 1 ? "," : ");\r\n"));
                sbSqlInsert.Append(column.ColumnName + (i != dt.Columns.Count - 1 ? "," : ") VALUES ("));
                i += 1;
            }
            foreach (DataRow row in dt.Rows)
            {
                i = 0;
                sbSqlRequest.Append(sbSqlInsert.ToString());
                foreach(DataColumn column in dt.Columns)
                {
                    sbSqlRequest.Append("'" + row[column].ToString() + "'" + (i != dt.Columns.Count - 1 ? "," : ");\r\n"));
                    i += 1;
                }
            }
            return sbSqlRequest.ToString();
        }
    }
}
