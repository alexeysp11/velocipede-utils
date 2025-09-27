using System.Data;
using Microsoft.Data.SqlClient;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// MS SQL database connection.
    /// </summary>
    public class MssqlDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.MSSQL;

        private string DataSource { get; set; }

        public MssqlDbConnection() { }

        public MssqlDbConnection(string dataSource)
        {
            DataSource = dataSource;
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            SqlConnection connection = null;
            try 
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = string.IsNullOrEmpty(DataSource) ? ConnectionString : DataSource;
                using (connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sqlRequest, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            table = GetDataTable(reader);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null) connection.Close();
            }
            return table;
        }

        public string GetSqlFromDataTable(DataTable dt, string tableName)
        {
            return GetSqlFromDataTable(dt, tableName);
        }

        private DataTable GetDataTable(SqlDataReader reader)
        {
            DataTable table = new DataTable();
            if (reader.FieldCount == 0) return table;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                DataColumn column;
                column = new DataColumn();
                column.ColumnName = reader.GetName(i);
                column.ReadOnly = true;
                table.Columns.Add(column);
            }
            while (reader.Read())
            {
                DataRow row = table.NewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                    row[i] = reader.GetValue(i).ToString();
                table.Rows.Add(row);
            }
            return table;
        }
    }
}
