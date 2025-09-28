using System;
using System.Data;
using System.Text;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Extension methods for <see cref="ICommonDbConnection"/>.
    /// </summary>
    public static class ICommonDbConnectionExtensions
    {
        /// <summary>
        /// Set connection string.
        /// </summary>
        public static ICommonDbConnection SetConnectionString(this ICommonDbConnection connection, string connectionString)
        {
            connection.ConnectionString = connectionString;
            return connection;
        }

        /// <summary>
        /// Gets SQL definition of the DataTable object.
        /// </summary>
        public static string GetSqlFromDataTable(this ICommonDbConnection connection, DataTable dt, string tableName)
        {
            if (dt == null)
                throw new ArgumentNullException(nameof(dt), "Data table could not be null");
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName), "Table name is not assigned");

            var sbSqlRequest = new StringBuilder();
            var sbSqlInsert = new StringBuilder();

            int i = 0;
            sbSqlRequest.Append("CREATE TABLE IF NOT EXISTS " + tableName + " (");
            sbSqlInsert.Append("INSERT INTO " + tableName + " (");
            foreach (DataColumn column in dt.Columns)
            {
                sbSqlRequest.Append("\n" + column.ColumnName + " TEXT" + (i != dt.Columns.Count - 1 ? "," : ");\n"));
                sbSqlInsert.Append(column.ColumnName + (i != dt.Columns.Count - 1 ? "," : ")\nVALUES ("));
                i += 1;
            }
            foreach (DataRow row in dt.Rows)
            {
                i = 0;
                sbSqlRequest.Append(sbSqlInsert.ToString());
                foreach(DataColumn column in dt.Columns)
                {
                    sbSqlRequest.Append("'" + row[column].ToString() + "'" + (i != dt.Columns.Count - 1 ? "," : ");\n"));
                    i += 1;
                }
            }
            return sbSqlRequest.ToString();
        }
    }
}
