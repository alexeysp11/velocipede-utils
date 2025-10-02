using System;
using System.Data;
using System.Text;

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
