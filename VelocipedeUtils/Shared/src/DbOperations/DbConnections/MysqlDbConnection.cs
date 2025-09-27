using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using VelocipedeUtils.Shared.DbOperations.Enums;

namespace VelocipedeUtils.Shared.DbOperations.DbConnections
{
    /// <summary>
    /// MySQL database connection.
    /// </summary>
    public class MysqlDbConnection : ICommonDbConnection
    {
        public string ConnectionString { get; set; }
        public DatabaseType DatabaseType => DatabaseType.MySQL;

        private string DataSource { get; set; }

        public MysqlDbConnection() { }

        public MysqlDbConnection(string dataSource)
        {
            DataSource = dataSource;
        }

        public DataTable ExecuteSqlCommand(string sqlRequest)
        {
            DataTable table = new DataTable();
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(string.IsNullOrEmpty(DataSource) ? ConnectionString : DataSource);
                connection.Open();
                var reader = new MySqlCommand(sqlRequest, connection).ExecuteReader();
                table = GetDataTable(reader);
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

        private DataTable GetDataTable(MySqlDataReader reader)
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
