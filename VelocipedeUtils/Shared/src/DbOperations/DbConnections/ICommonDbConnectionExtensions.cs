using System.Data;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// Extension methods for <see cref="ICommonDbConnection"/>.
    /// </summary>
    public static class ICommonDbConnectionExtensions
    {
        public static ICommonDbConnection SetConnectionString(this ICommonDbConnection connection, string connectionString)
        {
            connection.ConnectionString = connectionString;
            return connection;
        }

        public static string GetSqlFromDataTable(this ICommonDbConnection connection, DataTable dt, string tableName)
        {
            if (dt == null)
                throw new System.Exception("Data table could not be null");
            if (string.IsNullOrEmpty(tableName))
                throw new System.Exception("Table name is not assigned");

            int i = 0;
            string sqlRequest = "CREATE TABLE " + tableName + " (";
            string sqlInsert = "INSERT INTO " + tableName + " (";
            foreach (DataColumn column in dt.Columns)
            {
                sqlRequest += "\n" + column.ColumnName + " TEXT" + (i != dt.Columns.Count - 1 ? "," : ");\n");
                sqlInsert += column.ColumnName + (i != dt.Columns.Count - 1 ? "," : ")\nVALUES (");
                i += 1;
            }
            foreach (DataRow row in dt.Rows)
            {
                i = 0;
                sqlRequest += sqlInsert;
                foreach(DataColumn column in dt.Columns)
                {
                    sqlRequest += "'" + row[column].ToString() + "'" + (i != dt.Columns.Count - 1 ? "," : ");\n");
                    i += 1;
                }
            }
            return sqlRequest;
        }
    }
}
